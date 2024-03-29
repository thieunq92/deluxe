GO
/****** Object:  Table cuyahoga_site    Script Date: 02/27/2007 16:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TABLE cuyahoga_site NOCHECK CONSTRAINT ALL
GO
ALTER TABLE cuyahoga_user NOCHECK CONSTRAINT ALL
GO
ALTER TABLE cm_forumcategory NOCHECK CONSTRAINT ALL
GO
ALTER TABLE cm_forums NOCHECK CONSTRAINT ALL
GO
ALTER TABLE cm_forumposts NOCHECK CONSTRAINT ALL
GO
ALTER TABLE cm_forumuser NOCHECK CONSTRAINT ALL
GO
ALTER TABLE cm_forumfile NOCHECK CONSTRAINT ALL
GO
ALTER TABLE cm_forumcategory WITH NOCHECK ADD  CONSTRAINT [FK_cm_forumcategory_cuyahoga_site] FOREIGN KEY([siteid])
REFERENCES cuyahoga_site ([siteid])
GO
ALTER TABLE cm_forums  WITH CHECK ADD  CONSTRAINT [FK_cm_forums_cm_forumcategory] FOREIGN KEY([categoryid])
REFERENCES cm_forumcategory ([categoryid])
GO
ALTER TABLE cm_forumposts  WITH CHECK ADD  CONSTRAINT [FK_cm_forumposts_cm_forumfile1] FOREIGN KEY([attachmentid])
REFERENCES cm_forumfile ([id])
GO
ALTER TABLE cm_forumposts  WITH CHECK ADD  CONSTRAINT [FK_cm_forumposts_cm_forumposts] FOREIGN KEY([replytoid])
REFERENCES cm_forumposts ([postid])
GO
ALTER TABLE cm_forumposts  WITH CHECK ADD  CONSTRAINT [FK_cm_forumposts_cm_forums] FOREIGN KEY([forumid])
REFERENCES cm_forums ([forumid])
GO
ALTER TABLE cm_forumposts  WITH CHECK ADD  CONSTRAINT [FK_cm_forumposts_cuyahoga_user] FOREIGN KEY([userid])
REFERENCES cuyahoga_user ([userid])
GO
ALTER TABLE cm_forumuser  WITH NOCHECK ADD  CONSTRAINT [FK_cm_forumuser_cuyahoga_user] FOREIGN KEY([userid])
REFERENCES cuyahoga_user ([userid])
GO
ALTER TABLE cm_forumcategory CHECK CONSTRAINT ALL
GO
ALTER TABLE cm_forums CHECK CONSTRAINT ALL
GO
ALTER TABLE cm_forumposts CHECK CONSTRAINT ALL
GO
ALTER TABLE cm_forumuser CHECK CONSTRAINT ALL
GO
ALTER TABLE cuyahoga_site CHECK CONSTRAINT ALL
GO
ALTER TABLE cuyahoga_user CHECK CONSTRAINT ALL
GO
ALTER TABLE cm_forumfile CHECK CONSTRAINT ALL
GO
/*********************
Version updates
*********************/

UPDATE cuyahoga_version SET major = 1, minor = 5, patch = 1 WHERE assembly = 'Portal.Modules.Forum'
GO
