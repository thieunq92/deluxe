/*
 *  Remove module definitions
 */
 
DELETE FROM cuyahoga_version WHERE assembly = 'CMS.Modules.Gallery'
go
 
DELETE cuyahoga_modulesetting 
FROM cuyahoga_modulesetting ms
	INNER JOIN cuyahoga_moduletype mt ON mt.moduletypeid = ms.moduletypeid AND mt.assemblyname = 'CMS.Modules.Gallery'
go

DELETE cuyahoga_moduleservice
FROM cuyahoga_moduleservice ms
	INNER JOIN cuyahoga_moduletype mt ON mt.moduletypeid = ms.moduletypeid AND mt.assemblyname = 'CMS.Modules.Gallery'
go


DELETE FROM cuyahoga_moduletype
WHERE assemblyname = 'CMS.Modules.Gallery'
go

/*
 *  Remove module specific tables
 */

DROP TABLE cm_galleryphoto
go

DROP TABLE cm_galleryalbum
go


DELETE FROM cuyahoga_version WHERE assembly = 'CMS.Modules.GallerySidebar'
go

DELETE cuyahoga_modulesetting 
FROM cuyahoga_modulesetting ms
INNER JOIN cuyahoga_moduletype mt 
ON mt.moduletypeid = ms.moduletypeid 
AND mt.assemblyname = 'CMS.Modules.GallerySidebar'

go


DELETE FROM cuyahoga_moduletype
WHERE assemblyname = 'CMS.Modules.GallerySidebar'
go
