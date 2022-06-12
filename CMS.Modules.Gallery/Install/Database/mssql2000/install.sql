/*
 *  Table structure
 */
CREATE TABLE cm_galleryalbum(
albumid int identity(1,1) NOT NULL CONSTRAINT PK_album PRIMARY KEY,
sectionid int NOT NULL,
siteid int NOT NULL,
createdby int NOT NULL,
modifiedby int NULL,
title nvarchar(100) NULL,
description nvarchar(2000) NULL,
active bit NOT NULL,
photocount int NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go

CREATE TABLE cm_galleryphoto(
photoid int identity(1,1) NOT NULL CONSTRAINT PK_photo PRIMARY KEY,
sectionid int NOT NULL,
createdby int NOT NULL,
modifiedby int NULL,
albumid int NULL,
filepath nvarchar(255) NOT NULL,
title nvarchar(255) NULL,
filesize int NOT NULL,
nrofviews int NOT NULL,
imagewidth int NOT NULL,
imageheight int NOT NULL,
thumbwidth int NOT NULL,
thumbheight int NOT NULL,
rating float NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


ALTER TABLE cm_galleryalbum
ADD CONSTRAINT FK_album_site_siteid
FOREIGN KEY (siteid) REFERENCES bitportal_site (siteid)
go

ALTER TABLE cm_galleryalbum
ADD CONSTRAINT FK_album_section_sectionid
FOREIGN KEY (sectionid) REFERENCES bitportal_section (sectionid)
go

ALTER TABLE cm_galleryphoto
ADD CONSTRAINT FK_photo_album_albumid
FOREIGN KEY (albumid) REFERENCES cm_galleryalbum (albumid)
go

ALTER TABLE cm_galleryalbum
ADD CONSTRAINT FK_album_user_createdby
FOREIGN KEY (createdby) REFERENCES bitportal_user (userid)
go

ALTER TABLE cm_galleryalbum
ADD CONSTRAINT FK_album_user_modifiedby
FOREIGN KEY (modifiedby) REFERENCES bitportal_user (userid)
go

ALTER TABLE cm_galleryphoto
ADD CONSTRAINT FK_photo_user_createdby
FOREIGN KEY (createdby) REFERENCES bitportal_user (userid)
go

ALTER TABLE cm_galleryphoto
ADD CONSTRAINT FK_photo_user_modifiedby
FOREIGN KEY (modifiedby) REFERENCES bitportal_user (userid)
go


/*
 *  Table data
 */
SET DATEFORMAT ymd


DECLARE @moduletypeid int

INSERT INTO bitportal_moduletype ([name], assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES ('Gallery', 'CMS.Modules.Gallery', 'CMS.Modules.Gallery.GalleryModule', 'Modules/Gallery/Albums.ascx', 'Modules/Gallery/AdminGallery.aspx', '2004-10-02 14:36:28.324', '2004-10-02 14:36:28.324')

SELECT @moduletypeid = Scope_Identity()

INSERT INTO bitportal_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'PHYSICAL_DIR', 'Physical directory (~/UserFiles/Gallery)', 'System.String', 0, 0)
INSERT INTO bitportal_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'SHOW_VIEWS', 'Show photo view count', 'System.Boolean', 0, 0)
INSERT INTO bitportal_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'NUMBER_OF_COLUMNS', 'Number of columns to display', 'System.Int16', 0, 1)
INSERT INTO bitportal_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'NUMBER_OF_ITEMS_ON_PAGE', 'Number of items on a page', 'System.Int16', 0, 1)
INSERT INTO bitportal_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'MAX_IMAGE_DIMENSION', 'Maximum width and/or hight for images', 'System.Int16', 0, 1)
INSERT INTO bitportal_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'MAX_THUMB_DIMENSION', 'Maximum width and/or hight for thumbnails', 'System.Int16', 0, 1)
go

INSERT INTO bitportal_version (assembly, major, minor, patch) VALUES ('CMS.Modules.Gallery', 1, 5, 0)
go

INSERT INTO bitportal_moduletype ([name], assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) 
VALUES ('GallerySidebar', 'CMS.Modules.Gallery', 'CMS.Modules.Gallery.GallerySidebarModule', 
'Modules/Gallery/AlbumTitles.ascx', '', '2004-10-02 14:36:28.324', '2004-10-02 14:36:28.324')

DECLARE @moduletypeid int
SELECT @moduletypeid = Scope_Identity()

INSERT INTO bitportal_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'APPENDAND_GALLERY', 'Appendand Gallery Section', 'CMS.Modules.Gallery.AppendantGallery', 1, 1)
