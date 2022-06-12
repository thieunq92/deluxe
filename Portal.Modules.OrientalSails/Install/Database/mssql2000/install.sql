DECLARE @moduletypeid int
INSERT INTO bitportal_moduletype (name, assemblyname, classname, path, editpath) 
VALUES ('OrientalSails', 'Portal.Modules.OrientalSails', 'Portal.Modules.OrientalSails.SailsModule', 'Modules/Sails/Home.ascx', 'Modules/Sails/Admin/Home.aspx')

SELECT @moduletypeid = Scope_Identity()

INSERT INTO bitportal_moduleservice (moduletypeid, servicekey, servicetype, classtype)
VALUES (@moduletypeid, 'OrientalSails.ISailsDao', 'Portal.Modules.OrientalSails.DataAccess.ISailsDao, Portal.Modules.OrientalSails', 'Portal.Modules.OrientalSails.DataAccess.SailsDao, Portal.Modules.OrientalSails')

INSERT INTO bitportal_version (assembly, major, minor, patch) 
VALUES ('Portal.Modules.OrientalSails', 1, 0, 0)

INSERT INTO bitportal_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired)
VALUES (@moduletypeid, 'BABY_PRICE', 'Baby price (%)', 'System.Int32', 0, 1)

INSERT INTO bitportal_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired)
VALUES (@moduletypeid, 'CHILD_PRICE', 'Childrent price (%)', 'System.Int32', 0, 1)

INSERT INTO bitportal_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired)
VALUES (@moduletypeid, 'ADULT_PRICE', 'Adult price (%)', 'System.Int32', 0, 1)

INSERT INTO bitportal_globalsetting ( [Key],  [Value],  ModuleTypeId ) 
VALUES ('TICKET','0',  @moduletypeid)

INSERT INTO bitportal_globalsetting ( [Key],  [Value],  ModuleTypeId ) 
VALUES ('MEAL','0',  @moduletypeid)

INSERT INTO bitportal_globalsetting ( [Key],  [Value],  ModuleTypeId ) 
VALUES ('KAYAKING','0',  @moduletypeid)

INSERT INTO bitportal_globalsetting ( [Key],  [Value],  ModuleTypeId ) 
VALUES ('SERVICE','0',  @moduletypeid)

INSERT INTO bitportal_globalsetting ( [Key],  [Value],  ModuleTypeId ) 
VALUES ('RENT','0',  @moduletypeid)