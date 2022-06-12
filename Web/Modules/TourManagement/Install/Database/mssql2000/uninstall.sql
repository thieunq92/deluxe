DELETE cuyahoga_modulesetting 
FROM cuyahoga_modulesetting ms
	INNER JOIN cuyahoga_moduletype mt ON mt.moduletypeid = ms.moduletypeid AND mt.assemblyname = 'CMS.Modules.TourHotel'
go

DELETE FROM cuyahoga_version WHERE assembly = 'CMS.Modules.TourManagement'
go

/*DELETE FROM cuyahoga_moduleservice
WHERE servicekey = 'Tour.HotelDao'
GO*/
 
DELETE FROM cuyahoga_moduletype
WHERE assemblyname = 'CMS.Modules.TourManagement'
go