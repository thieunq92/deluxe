DELETE FROM cuyahoga_version WHERE assembly = 'Portal.Modules.Forum'
go
 
DELETE cuyahoga_modulesetting 
FROM cuyahoga_modulesetting ms
	INNER JOIN cuyahoga_moduletype mt ON mt.moduletypeid = ms.moduletypeid AND mt.assemblyname = 'Portal.Modules.Forum'
go
 
DELETE FROM cuyahoga_moduletype
WHERE assemblyname = 'Portal.Modules.Forum'
go

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cm_forumuser_cuyahoga_user]') AND parent_object_id = OBJECT_ID(N'[dbo].[cm_forumuser]'))
ALTER TABLE [dbo].[cm_forumuser] DROP CONSTRAINT [FK_cm_forumuser_cuyahoga_user]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cm_forumposts_cm_forumfile1]') AND parent_object_id = OBJECT_ID(N'[dbo].[cm_forumposts]'))
ALTER TABLE [dbo].[cm_forumposts] DROP CONSTRAINT [FK_cm_forumposts_cm_forumfile1]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cm_forumposts_cm_forumposts]') AND parent_object_id = OBJECT_ID(N'[dbo].[cm_forumposts]'))
ALTER TABLE [dbo].[cm_forumposts] DROP CONSTRAINT [FK_cm_forumposts_cm_forumposts]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cm_forumposts_cm_forums]') AND parent_object_id = OBJECT_ID(N'[dbo].[cm_forumposts]'))
ALTER TABLE [dbo].[cm_forumposts] DROP CONSTRAINT [FK_cm_forumposts_cm_forums]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cm_forumposts_cuyahoga_user]') AND parent_object_id = OBJECT_ID(N'[dbo].[cm_forumposts]'))
ALTER TABLE [dbo].[cm_forumposts] DROP CONSTRAINT [FK_cm_forumposts_cuyahoga_user]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cm_forums_cm_forumcategory]') AND parent_object_id = OBJECT_ID(N'[dbo].[cm_forums]'))
ALTER TABLE [dbo].[cm_forums] DROP CONSTRAINT [FK_cm_forums_cm_forumcategory]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cm_forumcategory_cuyahoga_site]') AND parent_object_id = OBJECT_ID(N'[dbo].[cm_forumcategory]'))
ALTER TABLE [dbo].[cm_forumcategory] DROP CONSTRAINT [FK_cm_forumcategory_cuyahoga_site]

GO


DROP TABLE cm_forums
go

DROP TABLE cm_forumposts
go

DROP TABLE cm_forumcategory
go

DROP TABLE cm_forumemoticon
go

DROP TABLE cm_forumtag
go

DROP TABLE cm_forumuser
go

DROP TABLE cm_forumfile
go

