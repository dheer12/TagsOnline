﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using TAGS_BARCODE_WEBAPP_V4.Models;
using TAGS_BARCODE_WEBAPP_V4.ViewModels;

namespace TAGS_BARCODE_WEBAPP_V4.APIControllers
{
    public class ScanController : ApiController
    {

        [Route("Dashboard/api/Station1")]
        [HttpPost]
        public CheckInVM CheckTicketStation1(CheckInVM checkInVM)
        {
            using (var db = new TagsDataModel())
            {
                var model = (from ticket in db.TICKETED_CHECKINS
                             where ticket.TICKET_NUMBER.Equals(checkInVM.TicketNo.ToString())
                             select ticket).FirstOrDefault();

                //checking in now. Ticket exists
                if (model != null && model.STATION_1 == false)
                {
                    model.STATION_1 = true;
                    model.STATION_1_CHECKIN_TIME = DateTime.Now;
                    //TODO find logged in userID
                    var user = (from loggedInUser in db.TAGS_LOGIN
                                where loggedInUser.LAST_NAME.ToLower().Equals(User.Identity.Name)
                                select loggedInUser).FirstOrDefault();
                    model.STATION_1_USER_ID = user.USER_ID;

                    checkInVM.IsCheckedIn = true;
                    checkInVM.TicketNotFound = false;
                    checkInVM.AlreadyCheckedIn = false;
                }
                //ticket exists.. Already checked in
                else if (model != null && model.STATION_1 == true)
                {
                    checkInVM.IsCheckedIn = false;
                    checkInVM.TicketNotFound = false;
                    checkInVM.AlreadyCheckedIn = true;

                }
                //ticket doesn't exist
                else if (model == null)
                {
                    checkInVM.IsCheckedIn = false;
                    checkInVM.TicketNotFound = true;
                    checkInVM.AlreadyCheckedIn = false;
                }
                db.SaveChanges();
                return checkInVM;
            }
        }

        [Route("Dashboard/api/AddUser")]
        [HttpPost]
        public void CreateUser(AddUserVM addUserVM)
        {
            using (var db = new TagsDataModel())
            {
                TAGS_LOGIN newUser = new TAGS_LOGIN();
                newUser.FIRST_NAME = addUserVM.FIRST_NAME;
                newUser.LAST_NAME = addUserVM.LAST_NAME;
                newUser.PASSWORD = addUserVM.PASSWORD;
                newUser.USER_ROLE = addUserVM.USER_ROLE;
                //don't know what this is for. Is it is logged in? 
                newUser.IS_LOGGED_ID = 0;
                db.TAGS_LOGIN.Add(newUser);
                db.SaveChanges();
            }
        }

        [Route("Dashboard/api/verifyUserName")]
        [HttpGet]
        public bool verifyUserName()
        {
            using (var db = new TagsDataModel())
            {
                //var userNameExists = from user in db.TAGS_USER
                //                     where user.
            }
            return false;
        }

        [Route("Dashboard/api/SearchMember")]
        [HttpPost]
        public MemberVM SearchMember(SearchMemberVM SearchMember)
        {
            using (var db = new TagsDataModel())
            {
                MemberVM memb = new MemberVM();
                var model = (from member in db.TAGS_MEMBER
                             where member.EMAIL_ID.ToLower().Equals(SearchMember.email.ToLower())
                             select member).FirstOrDefault();

                if (model != null)
                {
                    memb.CELL_PHONE = model.CELL_PHONE;
                    memb.COMPANY_NAME = model.COMPANY_NAME;
                    memb.EMAIL_ID = model.EMAIL_ID;
                    memb.FIRST_NAME = model.FIRST_NAME;
                    memb.HOME_PHONE = model.HOME_PHONE;
                    memb.IS_VOLUNTEER = model.IS_VOLUNTEER;
                    memb.LAST_NAME = model.LAST_NAME;
                    memb.MemberNotFound = false;
                    memb.MEMBER_ID = model.MEMBER_ID;
                }
                else
                {
                    memb.MemberNotFound = true;
                }

                return memb;
            }
        }


        [Route("Dashboard/api/UpdateMember")]
        [HttpPost]
        public void UpdateMember(MemberVM UpdateMember)
        {
            using (var db = new TagsDataModel())
            {
                var model = (from member in db.TAGS_MEMBER
                             where member.EMAIL_ID.ToLower().Equals(UpdateMember.EMAIL_ID.ToLower())
                             select member).FirstOrDefault();

                if (model != null)
                {
                    model.CELL_PHONE = UpdateMember.CELL_PHONE;
                    model.COMPANY_NAME = UpdateMember.COMPANY_NAME;
                    model.EMAIL_ID = UpdateMember.EMAIL_ID ;
                    model.FIRST_NAME = UpdateMember.FIRST_NAME ;
                    model.HOME_PHONE = UpdateMember.HOME_PHONE;
                    model.IS_VOLUNTEER = UpdateMember.IS_VOLUNTEER;
                    model.LAST_NAME = UpdateMember.LAST_NAME;
                    db.SaveChanges();
                }
            }
        }

        [Route("Dashboard/api/AddMember")]
        [HttpPost]
        public MemberVM AddMember(MemberVM AddMember)
        {
            using (var db = new TagsDataModel())
            {
                TAGS_MEMBER NewMember = new TAGS_MEMBER();
                NewMember.CELL_PHONE = AddMember.CELL_PHONE;
                NewMember.COMPANY_NAME = AddMember.COMPANY_NAME;
                NewMember.EMAIL_ID = AddMember.EMAIL_ID;
                NewMember.FIRST_NAME = AddMember.FIRST_NAME;
                NewMember.HOME_PHONE = AddMember.HOME_PHONE;
                NewMember.IS_VOLUNTEER = AddMember.IS_VOLUNTEER;
                NewMember.LAST_NAME = AddMember.LAST_NAME;
                db.TAGS_MEMBER.Add(NewMember);
                db.SaveChanges();

                MemberVM member = new MemberVM();
                member.CELL_PHONE = NewMember.CELL_PHONE;
                member.COMPANY_NAME = NewMember.COMPANY_NAME;
                member.EMAIL_ID = NewMember.EMAIL_ID;
                member.FIRST_NAME = NewMember.FIRST_NAME;
                member.HOME_PHONE = NewMember.HOME_PHONE;
                member.IS_VOLUNTEER = NewMember.IS_VOLUNTEER;
                member.LAST_NAME = NewMember.LAST_NAME;
                member.MemberNotFound = false;
                member.MEMBER_ID = NewMember.MEMBER_ID;

                return member;
            }
        }

    }
}