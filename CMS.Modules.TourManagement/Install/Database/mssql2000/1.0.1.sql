DECLARE @moduletypeid int

/* Register with KIT.CMS Module Manager*/
INSERT INTO cuyahoga_moduletype ([name],friendlyName, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) 
VALUES ('TourHotel','Hotel Management' ,'CMS.Modules.TourHotel', 'CMS.Modules.TourHotel.HotelModule', 'Modules/TourHotel/Hotel.ascx', 'Modules/TourHotel/Admin/Home.aspx', getdate(), getdate())
SELECT @moduletypeid = Scope_Identity()
 
/* Register Module Version */
INSERT INTO cuyahoga_version (assembly, major, minor, patch) 
VALUES ('CMS.Modules.TourHotel', 1, 0, 0)

/* Register Services */
INSERT INTO cuyahoga_moduleservice (moduletypeid, servicekey, servicetype, classtype) 
VALUES (@moduletypeid, 'Hotel.HotelDao', 'CMS.Modules.TourHotel.DataAccess.IHotelDao, CMS.Modules.TourHotel', 'CMS.Modules.TourHotel.DataAccess.HotelDao, CMS.Modules.TourHotel')

/* Register Module Setting */
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired)
VALUES (@moduletypeid, 'SHOW_PHONE', 'Show hotel phone on user view', 'System.Boolean', 0, 1)

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired)
VALUES (@moduletypeid, 'SHOW_EMAIL', 'Show hotel email on user view', 'System.Boolean', 0, 1)

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired)
VALUES (@moduletypeid, 'SHOW_FAX', 'Show hotel fax number on user view', 'System.Boolean', 0, 1)

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired)
VALUES (@moduletypeid, 'MAX_IMAGE_SIZE', 'Max image file size (in KB, 0 is unlimited)', 'System.Int32', 0, 1)

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired)
VALUES (@moduletypeid, 'MAX_CONTRACT_SIZE', 'Max contract file size (in KB, 0 is unlimited)', 'System.Int32', 0, 1)

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired)
VALUES (@moduletypeid, 'FROM_PRICE_ROOMTYPE', 'From Price will show the cheapest of', 'CMS.Modules.TourHotel.RoomTypeSetting', 1, 1)

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired)
VALUES (@moduletypeid, 'PAGE_SIZE', 'Number of hotels per page', 'System.Int32', 0, 1)

/* Register secondary ModuleType (sub module) */
INSERT INTO cuyahoga_moduletype ([name],friendlyName, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) 
VALUES ('TourHotel','Top Hotel', 'CMS.Modules.TourHotel', 'CMS.Modules.TourHotel.FeaturedHotelModule', 'Modules/TourHotel/FeaturedHotel.ascx', 'Modules/TourHotel/Admin/FeaturedHotel.aspx', getdate(), getdate())

SELECT @moduletypeid = Scope_Identity()

/* Register SubModule Setting */
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired)
VALUES (@moduletypeid, 'LOCATION', 'Show featured hotel in location', 'System.Int32', 0, 1)

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired)
VALUES (@moduletypeid, 'NUMBER_OF_HOTEL', 'Number of Featured Hotel should be show', 'System.Int32', 0, 1)

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired)
VALUES (@moduletypeid, 'SHOW_LOCATION_NAME', 'Show location name', 'System.Boolean', 0, 1)

/* Register secondary ModuleType (sub module) */
INSERT INTO cuyahoga_moduletype ([name],friendlyname, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) 
VALUES ('TourHotel','Hotel Search Input', 'CMS.Modules.TourHotel', 'CMS.Modules.TourHotel.HotelSearchInputModule', 'Modules/TourHotel/HotelSearchInput.ascx', null, getdate(), getdate())

SELECT @moduletypeid = Scope_Identity()

/* Register Hotel Search Input Setting */
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired)
VALUES (@moduletypeid, 'SHOW_LABEL', 'Show label before', 'System.Boolean', 0, 1)

/* Register secondary ModuleType: Location hotel list (sub module) */
INSERT INTO cuyahoga_moduletype ([name],friendlyname, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) 
VALUES ('TourHotel','Location hotel list', 'CMS.Modules.TourHotel', 'CMS.Modules.TourHotel.HotelQuickListModule', 'Modules/TourHotel/HotelQuickList.ascx', null, getdate(), getdate())

SELECT @moduletypeid = Scope_Identity()

/* Register Location hotel list Setting */
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired)
VALUES (@moduletypeid, 'NUMBER_OF_HOTEL', 'Number of hotel should be show', 'System.Int32', 0, 1)
