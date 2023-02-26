﻿using Gharbetti.Data;
using Gharbetti.Models;
using Gharbetti.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gharbetti.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HouseController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public HouseController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Route("Add")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] HouseViewModel model)
        {
            using (var dbContext = _db.Database.BeginTransaction())
            {
                try
                {

                    var addedHouse = await _db.Houses.AddAsync(new House
                    {
                        Name = model.Name,
                        Street = model.Street,
                        Address = model.Address,
                        Remarks = model.Remarks,
                        RentAmount = model.RentAmount,
                        SquareFootage = model.SquareFootage,
                    });
                    _db.SaveChanges();

                    foreach (var item in model.HouseRoomViewModels)
                    {
                        await _db.HouseRooms.AddAsync(new HouseRoom
                        {
                            HouseId = addedHouse.Entity.Id,
                            RoomId = item.Id,
                        });
                        _db.SaveChanges();
                    }

                    dbContext.Commit();
                    return Ok(new { Data = model, Status = true, Message = "House saved Sucessfully!!!" });
                }
                catch (Exception)
                {
                    dbContext.Rollback();
                    return Ok(new { Data = model, Status = false, Message = "Error while Saving!!!" });
                }
            }
        }

        [HttpGet]
        [Route("Edit")]
        public IActionResult Edit(int id)
        {
            var editData = _db.Houses.FirstOrDefault(x => x.Id == id);

            if(editData == null)
            {
                return Ok(new { Status = false, Message = "Data Not Found!!!" });
            }

            var houserooms = _db.HouseRooms.Where(x => x.HouseId == id).Select(x => new HouseRoomViewModel
            {
                Id = x.Id,
                HouseId = x.HouseId,
                RoomId = x.RoomId
            }).ToList();

            var houseEditData = new HouseViewModel
            {
                Id = editData.Id,
                Address = editData.Address,
                Name = editData.Name,
                Street = editData.Street,
                Remarks = editData.Remarks,
                RentAmount = editData.RentAmount,
                SquareFootage = editData.SquareFootage,
                HouseRoomViewModels = houserooms
            };

            if (editData != null)
            {
                return Ok(new { Data = houseEditData, Status = true });
            }
            else
            {
                return Ok(new { Status = false, Message = "Error Occured!!!" });
            }
        }


        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> Edit([FromBody] HouseViewModel model)
        {
            using (var dbContext = _db.Database.BeginTransaction())
            {
                try
                {
                    var savedEditData = await _db.Houses.FirstOrDefaultAsync(x => x.Id == model.Id);

                    if (savedEditData != null)
                    {
                        savedEditData.Remarks = model.Remarks;
                        savedEditData.SquareFootage = model.SquareFootage;
                        savedEditData.Address = model.Address;
                        savedEditData.Name = model.Name;
                        savedEditData.Street = model.Street;

                        _db.Houses.Update(savedEditData);
                        _db.SaveChanges();

                        var deleteData = _db.HouseRooms.Where(x => x.HouseId == model.Id).ToList();
                        if (deleteData.Count > 0)
                        {
                            _db.HouseRooms.RemoveRange(deleteData);
                        }

                        foreach (var item in model.HouseRoomViewModels)
                        {
                            await _db.HouseRooms.AddAsync(new HouseRoom
                            {
                                HouseId = model.Id,
                                RoomId = item.RoomId,
                            });
                            _db.SaveChanges();
                        }
                    }

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
            var editData = _db.Houses.FirstOrDefault(x => x.Id == id);

            if (editData != null)
            {
                using (var dbContext = _db.Database.BeginTransaction())
                {
                    try
                    {
                        _db.Houses.Remove(editData);
                        _db.SaveChanges();

                        var houseRoomData = _db.HouseRooms.Where(x => x.HouseId == editData.Id);

                        _db.HouseRooms.RemoveRange(houseRoomData);

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
    }
}
