﻿using CMS.Core.Domain;
using Portal.Modules.OrientalSails.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Domain
{
    public class Campaign
    {
        private int month;
        private int year;
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int Month
        {
            get
            {
                return month;
            }
            set
            {
                month = value;
                Name = SetDefaultName();
            }
        }
        public virtual int Year
        {
            get
            {
                return year;
            }
            set
            {
                year = value;
                Name = SetDefaultName();
            }
        }
        public virtual DateTime CreatedDate
        {
            get;
            private set;
        }
        public virtual User CreatedBy { get; set; }
        public virtual ICollection<Policy> Policies { get; set; }
        public Campaign()
        {
            Month = DateTime.Now.AddMonths(1).Month;
            Year = DateTime.Now.Year;
            Name = SetDefaultName();
            CreatedDate = DateTime.Now;
            Policies = new List<Policy>();
        }
        public virtual string SetDefaultName()
        {
            return "Campaign-" + Month.ToString("0#") + "-" + Year.ToString();
        }
        public virtual DateTime GetDateCampagin()
        {
            return new DateTime(Year, Month, 1);
        }
        public virtual string DateCampaignAsString
        {
            get
            {
                return GetDateCampagin().ToString("dd/MM/yyyy");
            }
        }
    }
}