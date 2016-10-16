using System;
using System.Web.Http;
using AutoMapper;
using Boongaloo.API.Automapper;
using Boongaloo.API.Helpers;
using Boongaloo.Repository.BoongalooDtos;
using Boongaloo.Repository.Entities;
using Boongaloo.Repository.UnitOfWork;

namespace Boongaloo.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/v1/groups")]
    public class GroupsController : ApiController
    {
        private BoongalooDbUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GroupsController( /*Comma separated arguments of type interface*/)
        {
            _unitOfWork = new BoongalooDbUnitOfWork();
            // Handle assignment by DI
            var mapperConfiguration = new MapperConfiguration(cfg => {
                cfg.AddProfile<BoongalooProfile>();
            });
            _mapper = mapperConfiguration.CreateMapper();
        }

        /// <summary>
        /// Example: GET /api/v1/groups/{lat:double}/{lon:double}/
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="lon">Longitude</param>
        /// <returns>All the groups that contain this point(lat/lon) as part of their diameter</returns>
        [HttpGet]
        [Route("{lat:double}/{lon:double}")]
        public IHttpActionResult Get(double lat, double lon)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = this._unitOfWork.GroupRepository.GetGroups(lat, lon);

                return Ok(result);
            }
            catch (Exception ex)
            {
                BoongalooApiLogger.LogError("Error while getting groups around coordinates.", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Example: POST /api/v1/groups
        /// </summary>
        /// <param name="newGroup">Body sample:{'Name':'Second floor cooks', 'TagIds':[4,1], 'AreaIds':[1],'UserIds':[1]}</param>
        /// <returns>HTTP Code 201 if successfuly created and 500 if not.</returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] StandaloneGroupRequestDto newGroup)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var groupAsEntity = this._mapper.Map<StandaloneGroupRequestDto, Group>(newGroup);
                var newlyCreatedGroupId = this._unitOfWork
                    .GroupRepository
                    .InsertGroup(
                        groupAsEntity,
                        newGroup.AreaIds,
                        newGroup.TagIds,
                        newGroup.UserIds);

                this._unitOfWork.Save();
                return Created("Success", "api/v1/groups/" + newlyCreatedGroupId);
            }
            catch (Exception ex)
            {
                BoongalooApiLogger.LogError("Error while inserting a new group.", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Example: POST /api/v1/groups/AsNewArea
        /// </summary>
        /// <param name="newGroup">Body sample:{'Name':'Second floor cooks', 'TagIds':[4,1], 'AreaIds':[1],'UserIds':[1],'Latitude':42.657064, 'Longitude':23.28539, 'Radius':50}</param>
        /// <returns>Uniqe identifier of the newly created group entity</returns>
        [HttpPost]
        [Route("AsNewArea")]
        public IHttpActionResult Post([FromBody] GroupAsNewAreaRequestDto newGroup)
        {
            try
            {
                // New Area was saved
                var areaAsEntity = this._mapper.Map<GroupAsNewAreaRequestDto, Area>(newGroup);
                this._unitOfWork.AreaRepository.InsertArea(areaAsEntity);

                var groupAsEntity = this._mapper.Map<GroupAsNewAreaRequestDto, Group>(newGroup);
                var newlyCreatedGroupId = this._unitOfWork
                    .GroupRepository
                    .InsertGroup(
                        groupAsEntity, 
                        newGroup.AreaIds, 
                        newGroup.TagIds, 
                        newGroup.UserIds);

                this._unitOfWork.Save();
                return Created("Success", "api/v1/groups/" + newlyCreatedGroupId);
            }
            catch (Exception ex)
            {
                BoongalooApiLogger.LogError("Error while creating new group.", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Example: GET /api/v1/groups/{id:int}
        /// </summary>
        /// <param name="id">Unique identifier of a group</param>
        /// <returns>Specific group by its id.</returns>
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _unitOfWork.GroupRepository.GetGroupById(id);

            return Ok(result);
        }

        /// <summary>
        /// Example: GET api/v1/groups/{id:int}/users
        /// </summary>
        /// <param name="id">Unique identifier of the group you are getting the users from</param>
        /// <returns>All the users for a specific group</returns>
        [HttpGet]
        [Route("{id:int}/users")]
        public IHttpActionResult GetUsers(int id)
        {
            try
            {
                var result = this._unitOfWork.UserRepository.GetUsersFromGroup(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                BoongalooApiLogger.LogError("Error while getting users for group.", ex);
                return InternalServerError();
            }
        }
    };
}
