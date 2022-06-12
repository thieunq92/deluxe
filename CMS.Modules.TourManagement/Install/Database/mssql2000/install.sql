DECLARE @moduletypeid int

/* Register with KIT.CMS Module Manager*/
INSERT INTO bitportal_moduletype ([name], assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) 
VALUES ('TourManagement', 'CMS.Modules.TourManagement', 'CMS.Modules.TourManagement.TourManagementModule', 'Modules/TourManagement/TourManagement.ascx', 'Modules/TourManagement/Admin/Home.aspx', getdate(), getdate())
SELECT @moduletypeid = Scope_Identity()

/* Register secondary ModuleType (sub module) */
INSERT INTO bitportal_moduletype ([name], friendlyName, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) 
VALUES ('TourManagement','Location Tree', 'CMS.Modules.TourManagement', 'CMS.Modules.TourManagement.TourManagementModule', 'Modules/TourManagement/LocationTree.ascx', null, getdate(), getdate())
 
/* Register Module Version */
INSERT INTO bitportal_version (assembly, major, minor, patch) 
VALUES ('CMS.Modules.TourManagement', 1, 0, 0)

/* Register Services */
INSERT INTO bitportal_moduleservice (moduletypeid, servicekey, servicetype, classtype) 
VALUES (@moduletypeid, 'Tour.TourDao', 'CMS.Modules.TourManagement.DataAccess.ITourDao, CMS.Modules.TourManagement', 'CMS.Modules.TourManagement.DataAccess.TourDao, CMS.Modules.TourManagement')

/* Register Module Setting */
INSERT INTO bitportal_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired)
VALUES (@moduletypeid, 'IS_TOUR', 'Is Tour', 'System.Boolean', 0, 1)
/*
INSERT INTO bitportal_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired)
VALUES (@moduletypeid, 'CATEGORY_ID', 'Category ID', 'System.Int32', 0, 1)
*/

INSERT INTO bitportal_moduletype ([name], friendlyName, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) 
VALUES ('TourManagement','Tour search input', 'CMS.Modules.TourManagement', 'CMS.Modules.TourManagement.TourSearchInputModule', 'Modules/TourManagement/TourSearchInput.ascx', null, getdate(), getdate())

INSERT INTO bitportal_moduletype ([name], friendlyName, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) 
VALUES ('TourManagement','Top tours', 'CMS.Modules.TourManagement', 'CMS.Modules.TourManagement.FeaturedTourModule', 'Modules/TourManagement/FeaturedTours.ascx', null, getdate(), getdate())
SELECT @moduletypeid = Scope_Identity()

INSERT INTO bitportal_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired)
VALUES (@moduletypeid, 'NUMBER_OF_TOURS', 'Number of tours to be show', 'System.Int32', 0, 1)

INSERT INTO bitportal_moduletype ([name], friendlyName, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) 
VALUES ('TourManagement','Tour menu', 'CMS.Modules.TourManagement', 'CMS.Modules.TourManagement.TourMenuModule', 'Modules/TourManagement/TourRegionMenu.ascx', null, getdate(), getdate())

SELECT @moduletypeid = Scope_Identity()

INSERT INTO bitportal_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired)
VALUES (@moduletypeid, 'MENU_TYPE', 'Type of menu for tours', 'CMS.Modules.TourManagement.TourMenuType', 1, 1)


GO