USE [st1001]
GO
/****** Object:  Table [dbo].[LayoutType]    Script Date: 11/21/2014 18:13:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LayoutType](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_LayoutType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ComponentType]    Script Date: 11/21/2014 18:13:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ComponentType](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Headline] [nvarchar](max) NOT NULL,
	[Description] [ntext] NOT NULL,
 CONSTRAINT [PK_ComponentType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Offer]    Script Date: 11/21/2014 18:13:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Offer](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[CreatedBy] [int] NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[ContentIDs] [nvarchar](max) NOT NULL,
	[HasProject] [bit] NOT NULL,
	[CreateByExternal] [nvarchar](255) NULL,
 CONSTRAINT [PK_Offer] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 11/21/2014 18:13:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](150) NOT NULL,
	[FirstName] [nvarchar](150) NOT NULL,
	[LastName] [nvarchar](150) NOT NULL,
	[Password] [nvarchar](150) NOT NULL,
	[Role] [int] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SectionType]    Script Date: 11/21/2014 18:13:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SectionType](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_SectionType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Project]    Script Date: 11/21/2014 18:13:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project](
	[ID] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[Archived] [bit] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[Locked] [bit] NOT NULL,
	[Approved] [bit] NOT NULL,
	[HasPrivateMediaAssets] [bit] NOT NULL,
 CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MediaAsset]    Script Date: 11/21/2014 18:13:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MediaAsset](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[URL] [nvarchar](1000) NOT NULL,
	[Type] [tinyint] NOT NULL,
	[IsExternal] [bit] NOT NULL,
	[ProjectID] [int] NULL,
	[IsPrivate] [bit] NOT NULL,
 CONSTRAINT [PK_MediaAsset] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Component]    Script Date: 11/21/2014 18:13:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Component](
	[ID] [int] NOT NULL,
	[ProjectID] [int] NOT NULL,
	[ContentID] [int] NOT NULL,
	[SectionID] [int] NOT NULL,
	[ComponentID] [int] NOT NULL,
	[Data] [ntext] NOT NULL,
	[Inactive] [bit] NOT NULL,
 CONSTRAINT [PK_Component] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Default [DF_Offer_UpdatedDate]    Script Date: 11/21/2014 18:13:54 ******/
ALTER TABLE [dbo].[Offer] ADD  CONSTRAINT [DF_Offer_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
/****** Object:  Default [DF_Table_1_ArchivedDate]    Script Date: 11/21/2014 18:13:54 ******/
ALTER TABLE [dbo].[Project] ADD  CONSTRAINT [DF_Table_1_ArchivedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
/****** Object:  Default [DF_User_Role]    Script Date: 11/21/2014 18:13:54 ******/
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_Role]  DEFAULT ((0)) FOR [Role]
GO
/****** Object:  ForeignKey [FK_Component_Project]    Script Date: 11/21/2014 18:13:54 ******/
ALTER TABLE [dbo].[Component]  WITH CHECK ADD  CONSTRAINT [FK_Component_Project] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Project] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Component] CHECK CONSTRAINT [FK_Component_Project]
GO
/****** Object:  ForeignKey [FK_MediaAsset_Project]    Script Date: 11/21/2014 18:13:54 ******/
ALTER TABLE [dbo].[MediaAsset]  WITH CHECK ADD  CONSTRAINT [FK_MediaAsset_Project] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Project] ([ID])
GO
ALTER TABLE [dbo].[MediaAsset] CHECK CONSTRAINT [FK_MediaAsset_Project]
GO
/****** Object:  ForeignKey [FK_Project_Offer]    Script Date: 11/21/2014 18:13:54 ******/
ALTER TABLE [dbo].[Project]  WITH CHECK ADD  CONSTRAINT [FK_Project_Offer] FOREIGN KEY([ID])
REFERENCES [dbo].[Offer] ([ID])
GO
ALTER TABLE [dbo].[Project] CHECK CONSTRAINT [FK_Project_Offer]
GO
/****** Object:  ForeignKey [FK_Project_User]    Script Date: 11/21/2014 18:13:54 ******/
ALTER TABLE [dbo].[Project]  WITH CHECK ADD  CONSTRAINT [FK_Project_User] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[User] ([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Project] CHECK CONSTRAINT [FK_Project_User]
GO
