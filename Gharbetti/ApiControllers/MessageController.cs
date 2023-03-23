using Gharbetti.Data;
using Gharbetti.Models;
using Gharbetti.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Gharbetti.ApiControllers.ExpenseController;

namespace Gharbetti.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public MessageController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Route("Add")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] MessageViewModel model)
        {
            var dbTran = _db.Database.BeginTransaction();
            try
            {
                var addedMessage = await _db.Message.AddAsync(new Message
                {
                    Subject = model.Subject,
                    Body = model.Body,
                    PostedDate = DateTime.Now,
                });

                _db.SaveChanges();

                if (model.HouseId == 0)
                {
                    var allUser = (from usr in _db.Users
                                   join userRole in _db.UserRoles on usr.Id equals userRole.UserId
                                   join role in _db.Roles on userRole.RoleId equals role.Id
                                   join ap in _db.ApplicationUsers on usr.Id equals ap.Id
                                   join room in _db.Rooms on ap.RoomId equals room.Id
                                   join hr in _db.HouseRooms on room.Id equals hr.RoomId
                                   join h in _db.Houses on hr.HouseId equals h.Id
                                   where role.Name.ToLower() == "tenant"
                                   select new
                                   {
                                       UserId = usr.Id,
                                   }).ToList();

                    foreach (var item in allUser)
                    {
                        await _db.TenantMessages.AddAsync(new TenantMessage
                        {
                            MessageId = addedMessage.Entity.Id,
                            TenantId = Guid.Parse(item.UserId),
                            Status = 1
                        });
                    }

                }
                else
                {
                    var allUser = (from usr in _db.Users
                                   join userRole in _db.UserRoles on usr.Id equals userRole.UserId
                                   join role in _db.Roles on userRole.RoleId equals role.Id
                                   join ap in _db.ApplicationUsers on usr.Id equals ap.Id
                                   join room in _db.Rooms on ap.RoomId equals room.Id
                                   join hr in _db.HouseRooms on room.Id equals hr.RoomId
                                   join h in _db.Houses on hr.HouseId equals h.Id
                                   where role.Name.ToLower() == "tenant" && h.Id == model.HouseId
                                   select new
                                   {
                                       UserId = usr.Id,
                                   }).ToList();

                    foreach (var item in allUser)
                    {
                        await _db.TenantMessages.AddAsync(new TenantMessage
                        {
                            MessageId = addedMessage.Entity.Id,
                            TenantId = Guid.Parse(item.UserId),
                            Status = 1
                        });
                    }


                }
                await _db.SaveChangesAsync();
                dbTran.Commit();
                return Ok(new { Data = model, Status = true, Message = "House saved Sucessfully!!!" });
            }
            catch (Exception)
            {
                dbTran.Rollback();
                return Ok(new { Data = model, Status = false, Message = "Error while Saving!!!" });
            }

        }

        [HttpGet]
        [Route("Edit")]
        public IActionResult Edit(int id)
        {
            var editData = _db.Message.FirstOrDefault(x => x.Id == id);

            if (editData == null)
            {
                return Ok(new { Status = false, Message = "Data Not Found!!!" });
            }

            return Ok(new { Data = editData, Status = true });

        }


        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> Edit([FromBody] MessageViewModel model)
        {
            using (var dbContext = _db.Database.BeginTransaction())
            {
                try
                {
                    var savedEditData = await _db.Message.FirstOrDefaultAsync(x => x.Id == model.Id);
                    var houseId = savedEditData?.HouseId;
                    if (savedEditData != null)
                    {
                        savedEditData.Subject = model.Subject;
                        savedEditData.Body = model.Body;
                        savedEditData.HouseId = model.HouseId;

                        _db.Message.Update(savedEditData);

                        if(houseId != model.HouseId)
                        {
                            var allHouseList = await _db.TenantMessages.Where(x => x.MessageId ==  model.Id).ToListAsync();

                            _db.TenantMessages.RemoveRange(allHouseList);
                        }

                        if (model.HouseId == 0)
                        {
                            var allUser = (from usr in _db.Users
                                           join userRole in _db.UserRoles on usr.Id equals userRole.UserId
                                           join role in _db.Roles on userRole.RoleId equals role.Id
                                           join ap in _db.ApplicationUsers on usr.Id equals ap.Id
                                           join room in _db.Rooms on ap.RoomId equals room.Id
                                           join hr in _db.HouseRooms on room.Id equals hr.RoomId
                                           join h in _db.Houses on hr.HouseId equals h.Id
                                           where role.Name.ToLower() == "tenant"
                                           select new
                                           {
                                               UserId = usr.Id,
                                           }).ToList();

                            foreach (var item in allUser)
                            {
                                await _db.TenantMessages.AddAsync(new TenantMessage
                                {
                                    MessageId = savedEditData.Id,
                                    TenantId = Guid.Parse(item.UserId),
                                    Status = 1
                                });
                            }

                        }
                        else
                        {
                            var allUser = (from usr in _db.Users
                                           join userRole in _db.UserRoles on usr.Id equals userRole.UserId
                                           join role in _db.Roles on userRole.RoleId equals role.Id
                                           join ap in _db.ApplicationUsers on usr.Id equals ap.Id
                                           join room in _db.Rooms on ap.RoomId equals room.Id
                                           join hr in _db.HouseRooms on room.Id equals hr.RoomId
                                           join h in _db.Houses on hr.HouseId equals h.Id
                                           where role.Name.ToLower() == "tenant" && h.Id == model.HouseId
                                           select new
                                           {
                                               UserId = usr.Id,
                                           }).ToList();

                            foreach (var item in allUser)
                            {
                                await _db.TenantMessages.AddAsync(new TenantMessage
                                {
                                    MessageId = savedEditData.Id,
                                    TenantId = Guid.Parse(item.UserId),
                                    Status = 1
                                });
                            }


                        }
                    }

                    _db.SaveChanges();
                    dbContext.Commit();
                    return Ok(new { Data = model, Status = true, Message = "Data Updated Successfully!!!" });
                }
                catch (Exception ex)
                {
                    dbContext.Rollback();
                    return Ok(new { Data = model, Status = false, Message = "Error Occured" });
                }
            }
        }


        [HttpGet]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            var editData = _db.Message.FirstOrDefault(x => x.Id == id);

            if (editData != null)
            {
                using (var dbContext = _db.Database.BeginTransaction())
                {
                    try
                    {
                        _db.Message.Remove(editData);

                        var tenantMessageData = _db.TenantMessages.Where(x => x.MessageId == editData.Id);
                        _db.TenantMessages.RemoveRange(tenantMessageData);


                        _db.SaveChanges();
                        dbContext.Commit();
                        return Ok(new { Data = editData, Status = true, Message = "Deleted Successfully!!!" });
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        return Ok(new { Data = editData, Status = false, Message = "Error Occured!!!" });
                    }
                }
            }
            else
            {
                return Ok(new { Status = false, Message = "Data Not Found!!!" });
            }
        }


        [HttpGet]
        [Route("GetHouses")]
        public IActionResult GetHouses()
        {

            var houseList = _db.Houses.ToList();

            return Ok(new { Status = true, Message = "Data Load Sucessfully", Data = houseList });
        }
    }
}
