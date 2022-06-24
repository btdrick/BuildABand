USE [master]
GO

DROP DATABASE BuildABand
GO

/****** Object:  Database [BuildABand]    Script Date: 18/06/2022 6:40:34 PM ******/
CREATE DATABASE [BuildABand]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BuildABand', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\BuildABand.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BuildABand_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\BuildABand_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [BuildABand] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BuildABand].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BuildABand] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BuildABand] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BuildABand] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BuildABand] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BuildABand] SET ARITHABORT OFF 
GO
ALTER DATABASE [BuildABand] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [BuildABand] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BuildABand] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BuildABand] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BuildABand] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BuildABand] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BuildABand] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BuildABand] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BuildABand] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BuildABand] SET  ENABLE_BROKER 
GO
ALTER DATABASE [BuildABand] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BuildABand] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BuildABand] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BuildABand] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BuildABand] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BuildABand] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [BuildABand] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BuildABand] SET RECOVERY FULL 
GO
ALTER DATABASE [BuildABand] SET  MULTI_USER 
GO
ALTER DATABASE [BuildABand] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BuildABand] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BuildABand] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BuildABand] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [BuildABand] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [BuildABand] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'BuildABand', N'ON'
GO
ALTER DATABASE [BuildABand] SET QUERY_STORE = OFF
GO
USE [BuildABand]
GO
/****** Object:  Table [dbo].[Accounts]    Script Date: 18/06/2022 6:40:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accounts](
	[AccountID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[is_Active] [tinyint] NOT NULL,
 CONSTRAINT [PK_AccountID] PRIMARY KEY CLUSTERED 
(
	[AccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Audio]    Script Date: 18/06/2022 6:40:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Audio](
	[AudioID] [int] IDENTITY(1,1) NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[FileName] [int] NOT NULL,
	[OwnerID] [int] NOT NULL,
 CONSTRAINT [PK_AudioID] PRIMARY KEY CLUSTERED 
(
	[AudioID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comment]    Script Date: 18/06/2022 6:40:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comment](
	[CommentID] [int] IDENTITY(1,1) NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[ParentID] [int] NULL,
	[MusicianID] [int] NOT NULL,
	[PostID] [int] NOT NULL,
	[Content] [varchar](100) NOT NULL,
 CONSTRAINT [PK_CommentID] PRIMARY KEY CLUSTERED 
(
	[CommentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Connection]    Script Date: 18/06/2022 6:40:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Connection](
	[ConnectionID] [int] IDENTITY(1,1) NOT NULL,
	[InitiatorID] [int] NOT NULL,
	[FollowerID] [int] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[Connected] [tinyint] NOT NULL,
 CONSTRAINT [PK_ConnectionID] PRIMARY KEY CLUSTERED 
(
	[ConnectionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Message]    Script Date: 18/06/2022 6:40:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Message](
	[MessageID] [int] IDENTITY(1,1) NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[SenderID] [int] NOT NULL,
	[RecipientID] [int] NOT NULL,
	[Subject] [varchar](100) NOT NULL,
	[Body] [varchar](500) NOT NULL,
 CONSTRAINT [PK_MessageID] PRIMARY KEY CLUSTERED 
(
	[MessageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Musician]    Script Date: 18/06/2022 6:40:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Musician](
	[MusicianID] [int] IDENTITY(1,1) NOT NULL,
	[Fname] [varchar](50) NOT NULL,
	[Lname] [varchar](50) NOT NULL,
	[DateOfBirth] [date] NOT NULL,
	[Phone] [varchar](12) NOT NULL,
	[Email] [varchar](200) NOT NULL,
	[Instrument] [varchar](500) NULL,
	[Sex] [varchar](10) NOT NULL,
	[Address1] [varchar](50) NOT NULL,
	[Address2] [varchar](50) NULL,
	[City] [varchar](50) NOT NULL,
	[ZipCode] [varchar](20) NOT NULL,
	[AccountID] [int] NOT NULL,
	[StateCode] [char](2) NOT NULL,
	[AvaterFilename] [varchar](200) NULL,
 CONSTRAINT [PK_MusicianID] PRIMARY KEY CLUSTERED 
(
	[MusicianID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Post]    Script Date: 18/06/2022 6:40:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Post](
	[PostID] [int] IDENTITY(1,1) NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[MusicianID] [int] NOT NULL,
	[Content] [varchar](100) NOT NULL,
 CONSTRAINT [PK_PostID] PRIMARY KEY CLUSTERED 
(
	[PostID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Project]    Script Date: 18/06/2022 6:40:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project](
	[ProjectID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[OwnerID] [int] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_ProjectID] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Project_Workon]    Script Date: 18/06/2022 6:40:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project_Workon](
	[ProjectID] [int] NOT NULL,
	[MusicianID] [int] NOT NULL,
	[JoinedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_ProjectID_MusicianID] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[MusicianID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[States]    Script Date: 18/06/2022 6:40:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[States](
	[StateCode] [char](2) NOT NULL,
	[StateName] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_State] PRIMARY KEY CLUSTERED 
(
	[StateCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Accounts] ON 

INSERT [dbo].[Accounts] ([AccountID], [Username], [Password], [is_Active]) VALUES (1, N'dnancy', N'g0pwm6JTTr4+4Tl/1Pe9KIsqzB0goI1shi3NmbbwRAA=', 1)
INSERT [dbo].[Accounts] ([AccountID], [Username], [Password], [is_Active]) VALUES (2, N'fandrew', N'g0pwm6JTTr4+4Tl/1Pe9KIsqzB0goI1shi3NmbbwRAA=', 1)
INSERT [dbo].[Accounts] ([AccountID], [Username], [Password], [is_Active]) VALUES (3, N'ljanet', N'g0pwm6JTTr4+4Tl/1Pe9KIsqzB0goI1shi3NmbbwRAA=', 1)
INSERT [dbo].[Accounts] ([AccountID], [Username], [Password], [is_Active]) VALUES (4, N'pmargaret', N'g0pwm6JTTr4+4Tl/1Pe9KIsqzB0goI1shi3NmbbwRAA=', 1)
INSERT [dbo].[Accounts] ([AccountID], [Username], [Password], [is_Active]) VALUES (5, N'bsteven', N'g0pwm6JTTr4+4Tl/1Pe9KIsqzB0goI1shi3NmbbwRAA=', 1)
INSERT [dbo].[Accounts] ([AccountID], [Username], [Password], [is_Active]) VALUES (6, N'smichael', N'g0pwm6JTTr4+4Tl/1Pe9KIsqzB0goI1shi3NmbbwRAA=', 1)
SET IDENTITY_INSERT [dbo].[Accounts] OFF
GO
SET IDENTITY_INSERT [dbo].[Comment] ON 

INSERT [dbo].[Comment] ([CommentID], [CreatedTime], [ParentID], [MusicianID], [PostID], [Content]) VALUES (1, CAST(N'2022-05-20T00:05:00.000' AS DateTime), NULL, 3, 1, N'Thank you')
INSERT [dbo].[Comment] ([CommentID], [CreatedTime], [ParentID], [MusicianID], [PostID], [Content]) VALUES (2, CAST(N'2022-05-20T00:05:00.000' AS DateTime), NULL, 4, 1, N'Glad to be on this platform')
INSERT [dbo].[Comment] ([CommentID], [CreatedTime], [ParentID], [MusicianID], [PostID], [Content]) VALUES (3, CAST(N'2022-05-20T00:05:00.000' AS DateTime), NULL, 4, 2, N'Wow! never did listen to it')
INSERT [dbo].[Comment] ([CommentID], [CreatedTime], [ParentID], [MusicianID], [PostID], [Content]) VALUES (4, CAST(N'2022-05-20T00:05:00.000' AS DateTime), 3, 5, 2, N'I do have it on my playlist')
INSERT [dbo].[Comment] ([CommentID], [CreatedTime], [ParentID], [MusicianID], [PostID], [Content]) VALUES (5, CAST(N'2022-05-20T00:05:00.000' AS DateTime), NULL, 1, 2, N'I total agree with you')
SET IDENTITY_INSERT [dbo].[Comment] OFF
GO
SET IDENTITY_INSERT [dbo].[Connection] ON 

INSERT [dbo].[Connection] ([ConnectionID], [InitiatorID], [FollowerID], [CreatedTime], [Connected]) VALUES (1, 1, 2, CAST(N'2022-02-09T00:00:00.000' AS DateTime), 1)
INSERT [dbo].[Connection] ([ConnectionID], [InitiatorID], [FollowerID], [CreatedTime], [Connected]) VALUES (2, 1, 3, CAST(N'2022-02-09T00:00:00.000' AS DateTime), 0)
INSERT [dbo].[Connection] ([ConnectionID], [InitiatorID], [FollowerID], [CreatedTime], [Connected]) VALUES (3, 1, 4, CAST(N'2022-02-09T00:00:00.000' AS DateTime), 1)
INSERT [dbo].[Connection] ([ConnectionID], [InitiatorID], [FollowerID], [CreatedTime], [Connected]) VALUES (4, 1, 5, CAST(N'2022-06-09T00:00:00.000' AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[Connection] OFF
GO
SET IDENTITY_INSERT [dbo].[Message] ON 

INSERT [dbo].[Message] ([MessageID], [CreatedTime], [SenderID], [RecipientID], [Subject], [Body]) VALUES (1, CAST(N'2022-04-20T00:00:00.000' AS DateTime), 1, 2, N'Happy bday', N'Happy bday Andrew')
INSERT [dbo].[Message] ([MessageID], [CreatedTime], [SenderID], [RecipientID], [Subject], [Body]) VALUES (2, CAST(N'2022-04-21T00:00:00.000' AS DateTime), 3, 2, N'Invitation Request', N'Would like to join my music project: Sweet Taboo')
INSERT [dbo].[Message] ([MessageID], [CreatedTime], [SenderID], [RecipientID], [Subject], [Body]) VALUES (3, CAST(N'2022-04-23T00:00:00.000' AS DateTime), 4, 1, N'Invitation Request', N'Would like to join my music project: Message In A Bottle')
SET IDENTITY_INSERT [dbo].[Message] OFF
GO
SET IDENTITY_INSERT [dbo].[Musician] ON 

INSERT [dbo].[Musician] ([MusicianID], [Fname], [Lname], [DateOfBirth], [Phone], [Email], [Instrument], [Sex], [Address1], [Address2], [City], [ZipCode], [AccountID], [StateCode], [AvaterFilename]) VALUES (1, N'Davolio', N'Nancy', CAST(N'1968-12-08' AS Date), N'206-555-9857', N'dnacy@test.com', N'Drum', N'Female', N'507 - 20th Ave.', NULL, N'Seattle', N'98122', 1, N'WA', NULL)
INSERT [dbo].[Musician] ([MusicianID], [Fname], [Lname], [DateOfBirth], [Phone], [Email], [Instrument], [Sex], [Address1], [Address2], [City], [ZipCode], [AccountID], [StateCode], [AvaterFilename]) VALUES (2, N'Fuller', N'Andrew', CAST(N'1952-02-19' AS Date), N'206-555-9482', N'fandrew@test.com', N'Piano', N'Male', N'908 W. Capital Way', N'by Zip way', N'Tacoma', N'98401', 2, N'WA', NULL)
INSERT [dbo].[Musician] ([MusicianID], [Fname], [Lname], [DateOfBirth], [Phone], [Email], [Instrument], [Sex], [Address1], [Address2], [City], [ZipCode], [AccountID], [StateCode], [AvaterFilename]) VALUES (3, N'Leverling', N'Janet', CAST(N'1963-08-30' AS Date), N'206-555-3412', N'ljanet@test.com', N'Bass Guitar', N'Female', N'722 Moss Bay Blvd.', NULL, N'Kirkland', N'98033', 3, N'WA', NULL)
INSERT [dbo].[Musician] ([MusicianID], [Fname], [Lname], [DateOfBirth], [Phone], [Email], [Instrument], [Sex], [Address1], [Address2], [City], [ZipCode], [AccountID], [StateCode], [AvaterFilename]) VALUES (4, N'Peacock', N'Margaret', CAST(N'1958-09-19' AS Date), N'206-555-8122', N'pmargaret@test.com', N'Lead Guitar', N'Female', N'4110 Old Redmond Rd.', NULL, N'Redmond', N'98052', 4, N'WA', NULL)
INSERT [dbo].[Musician] ([MusicianID], [Fname], [Lname], [DateOfBirth], [Phone], [Email], [Instrument], [Sex], [Address1], [Address2], [City], [ZipCode], [AccountID], [StateCode], [AvaterFilename]) VALUES (5, N'Buchanan', N'Steven', CAST(N'1955-03-04' AS Date), N'715-554-8480', N'bsteven@test.com', N'Drum', N'Male', N'14 Garrett Hill', N'SW1 8JR', N'Clint', N'87892', 5, N'NJ', NULL)
INSERT [dbo].[Musician] ([MusicianID], [Fname], [Lname], [DateOfBirth], [Phone], [Email], [Instrument], [Sex], [Address1], [Address2], [City], [ZipCode], [AccountID], [StateCode], [AvaterFilename]) VALUES (6, N'Suyama', N'Michael', CAST(N'1963-07-02' AS Date), N'715-557-7730', N'smichael@test.com', N'Trumpet', N'Male', N'331 Park Ave S', NULL, N'New York', N'86692', 6, N'NY', NULL)
SET IDENTITY_INSERT [dbo].[Musician] OFF
GO
SET IDENTITY_INSERT [dbo].[Post] ON 

INSERT [dbo].[Post] ([PostID], [CreatedTime], [MusicianID], [Content]) VALUES (1, CAST(N'2022-05-20T00:00:00.000' AS DateTime), 1, N'Welcome everyone to Build-a-band')
INSERT [dbo].[Post] ([PostID], [CreatedTime], [MusicianID], [Content]) VALUES (2, CAST(N'2022-05-21T00:00:00.000' AS DateTime), 2, N'I think FAR OUT album by police  is a need to listen to')
SET IDENTITY_INSERT [dbo].[Post] OFF
GO
SET IDENTITY_INSERT [dbo].[Project] ON 

INSERT [dbo].[Project] ([ProjectID], [Name], [OwnerID], [StartDate], [EndDate]) VALUES (1, N'Rolling Stone', 2, CAST(N'2022-02-08T00:00:00.000' AS DateTime), CAST(N'2022-04-19T00:00:00.000' AS DateTime))
INSERT [dbo].[Project] ([ProjectID], [Name], [OwnerID], [StartDate], [EndDate]) VALUES (2, N'Coming Up', 1, CAST(N'2022-04-18T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[Project] ([ProjectID], [Name], [OwnerID], [StartDate], [EndDate]) VALUES (3, N'Code A little', 1, CAST(N'2022-01-08T00:00:00.000' AS DateTime), CAST(N'2022-06-08T00:00:00.000' AS DateTime))
INSERT [dbo].[Project] ([ProjectID], [Name], [OwnerID], [StartDate], [EndDate]) VALUES (4, N'Message In A Bottle', 4, CAST(N'2022-06-08T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[Project] ([ProjectID], [Name], [OwnerID], [StartDate], [EndDate]) VALUES (5, N'Sweet Taboo', 3, CAST(N'2022-03-08T00:00:00.000' AS DateTime), CAST(N'2022-05-28T00:00:00.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[Project] OFF
GO
INSERT [dbo].[Project_Workon] ([ProjectID], [MusicianID], [JoinedDate]) VALUES (1, 1, CAST(N'2022-02-09T00:00:00.000' AS DateTime))
INSERT [dbo].[Project_Workon] ([ProjectID], [MusicianID], [JoinedDate]) VALUES (1, 2, CAST(N'2022-02-08T00:00:00.000' AS DateTime))
INSERT [dbo].[Project_Workon] ([ProjectID], [MusicianID], [JoinedDate]) VALUES (2, 1, CAST(N'2022-04-18T00:00:00.000' AS DateTime))
INSERT [dbo].[Project_Workon] ([ProjectID], [MusicianID], [JoinedDate]) VALUES (2, 2, CAST(N'2022-04-19T00:00:00.000' AS DateTime))
INSERT [dbo].[Project_Workon] ([ProjectID], [MusicianID], [JoinedDate]) VALUES (2, 3, CAST(N'2022-04-20T00:00:00.000' AS DateTime))
INSERT [dbo].[Project_Workon] ([ProjectID], [MusicianID], [JoinedDate]) VALUES (3, 1, CAST(N'2022-01-08T00:00:00.000' AS DateTime))
INSERT [dbo].[Project_Workon] ([ProjectID], [MusicianID], [JoinedDate]) VALUES (4, 1, CAST(N'2022-06-08T00:00:00.000' AS DateTime))
INSERT [dbo].[Project_Workon] ([ProjectID], [MusicianID], [JoinedDate]) VALUES (4, 4, CAST(N'2022-06-08T00:00:00.000' AS DateTime))
INSERT [dbo].[Project_Workon] ([ProjectID], [MusicianID], [JoinedDate]) VALUES (5, 3, CAST(N'2022-03-08T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'AK', N'Alaska')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'AL', N'Alabama')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'AR', N'Arkansas')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'AZ', N'Arizona')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'CA', N'California')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'CO', N'Colorado')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'CT', N'Connecticut')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'DC', N'District of Columbia')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'DE', N'Delaware')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'FL', N'Florida')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'GA', N'Georgia')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'HI', N'Hawaii')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'IA', N'Iowa')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'ID', N'Idaho')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'IL', N'Illinois')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'IN', N'Indiana')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'KS', N'Kansas')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'KY', N'Kentucky')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'LA', N'Lousiana')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'MA', N'Massachusetts')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'MD', N'Maryland')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'ME', N'Maine')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'MI', N'Michigan')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'MN', N'Minnesota')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'MO', N'Missouri')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'MS', N'Mississippi')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'MT', N'Montana')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'NC', N'North Carolina')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'ND', N'North Dakota')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'NE', N'Nebraska')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'NH', N'New Hampshire')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'NJ', N'New Jersey')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'NM', N'New Mexico')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'NV', N'Nevada')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'NY', N'New York')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'OH', N'Ohio')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'OK', N'Oklahoma')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'OR', N'Oregon')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'PA', N'Pennsylvania')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'RI', N'Rhode Island')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'SC', N'South Carolina')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'SD', N'South Dakota')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'TN', N'Tennessee')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'TX', N'Texas')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'UT', N'Utah')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'VA', N'Virginia')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'VI', N'Virgin Islands')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'VT', N'Vermont')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'WA', N'Washington')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'WI', N'Wisconsin')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'WV', N'West Virginia')
INSERT [dbo].[States] ([StateCode], [StateName]) VALUES (N'WY', N'Wyoming')
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UK_Accounts_Username]    Script Date: 18/06/2022 6:40:34 PM ******/
ALTER TABLE [dbo].[Accounts] ADD  CONSTRAINT [UK_Accounts_Username] UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UK_Connection_InitiatorID_FollowerID]    Script Date: 18/06/2022 6:40:34 PM ******/
ALTER TABLE [dbo].[Connection] ADD  CONSTRAINT [UK_Connection_InitiatorID_FollowerID] UNIQUE NONCLUSTERED 
(
	[InitiatorID] ASC,
	[FollowerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Audio]  WITH CHECK ADD  CONSTRAINT [FK_Audio_Musician] FOREIGN KEY([OwnerID])
REFERENCES [dbo].[Musician] ([MusicianID])
GO
ALTER TABLE [dbo].[Audio] CHECK CONSTRAINT [FK_Audio_Musician]
GO
ALTER TABLE [dbo].[Comment]  WITH CHECK ADD  CONSTRAINT [FK_Comment_Comment] FOREIGN KEY([ParentID])
REFERENCES [dbo].[Comment] ([CommentID])
GO
ALTER TABLE [dbo].[Comment] CHECK CONSTRAINT [FK_Comment_Comment]
GO
ALTER TABLE [dbo].[Comment]  WITH CHECK ADD  CONSTRAINT [FK_Comment_Musician] FOREIGN KEY([MusicianID])
REFERENCES [dbo].[Musician] ([MusicianID])
GO
ALTER TABLE [dbo].[Comment] CHECK CONSTRAINT [FK_Comment_Musician]
GO
ALTER TABLE [dbo].[Comment]  WITH CHECK ADD  CONSTRAINT [FK_Comment_Post] FOREIGN KEY([PostID])
REFERENCES [dbo].[Post] ([PostID])
GO
ALTER TABLE [dbo].[Comment] CHECK CONSTRAINT [FK_Comment_Post]
GO
ALTER TABLE [dbo].[Connection]  WITH CHECK ADD  CONSTRAINT [FK_Connection_Musician_FollowerID] FOREIGN KEY([FollowerID])
REFERENCES [dbo].[Musician] ([MusicianID])
GO
ALTER TABLE [dbo].[Connection] CHECK CONSTRAINT [FK_Connection_Musician_FollowerID]
GO
ALTER TABLE [dbo].[Connection]  WITH CHECK ADD  CONSTRAINT [FK_Connection_Musician_InitiatorID] FOREIGN KEY([InitiatorID])
REFERENCES [dbo].[Musician] ([MusicianID])
GO
ALTER TABLE [dbo].[Connection] CHECK CONSTRAINT [FK_Connection_Musician_InitiatorID]
GO
ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_Musician_RecipientID] FOREIGN KEY([RecipientID])
REFERENCES [dbo].[Musician] ([MusicianID])
GO
ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_Musician_RecipientID]
GO
ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_Musician_SenderID] FOREIGN KEY([SenderID])
REFERENCES [dbo].[Musician] ([MusicianID])
GO
ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_Musician_SenderID]
GO
ALTER TABLE [dbo].[Musician]  WITH CHECK ADD  CONSTRAINT [FK_Musician_Accounts] FOREIGN KEY([AccountID])
REFERENCES [dbo].[Accounts] ([AccountID])
GO
ALTER TABLE [dbo].[Musician] CHECK CONSTRAINT [FK_Musician_Accounts]
GO
ALTER TABLE [dbo].[Musician]  WITH CHECK ADD  CONSTRAINT [FK_Musician_States] FOREIGN KEY([StateCode])
REFERENCES [dbo].[States] ([StateCode])
GO
ALTER TABLE [dbo].[Musician] CHECK CONSTRAINT [FK_Musician_States]
GO
ALTER TABLE [dbo].[Post]  WITH CHECK ADD  CONSTRAINT [FK_Post_Musician] FOREIGN KEY([MusicianID])
REFERENCES [dbo].[Musician] ([MusicianID])
GO
ALTER TABLE [dbo].[Post] CHECK CONSTRAINT [FK_Post_Musician]
GO
ALTER TABLE [dbo].[Project]  WITH CHECK ADD  CONSTRAINT [FK_Project_Musician] FOREIGN KEY([OwnerID])
REFERENCES [dbo].[Musician] ([MusicianID])
GO
ALTER TABLE [dbo].[Project] CHECK CONSTRAINT [FK_Project_Musician]
GO
ALTER TABLE [dbo].[Project_Workon]  WITH CHECK ADD  CONSTRAINT [FK_Project_Workon_Musician] FOREIGN KEY([MusicianID])
REFERENCES [dbo].[Musician] ([MusicianID])
GO
ALTER TABLE [dbo].[Project_Workon] CHECK CONSTRAINT [FK_Project_Workon_Musician]
GO
ALTER TABLE [dbo].[Project_Workon]  WITH CHECK ADD  CONSTRAINT [FK_Project_Workon_Project] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Project] ([ProjectID])
GO
ALTER TABLE [dbo].[Project_Workon] CHECK CONSTRAINT [FK_Project_Workon_Project]
GO
/****** Object:  StoredProcedure [dbo].[createUser]    Script Date: 18/06/2022 6:40:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[createUser] 
(
	@Username varchar(50),
	@Password varchar(50),
	@Fname varchar(50),
	@Lname varchar(50),
	@DateOfBirth date,
	@Phone varchar(50),
	@Email varchar(200),
	@Instrument varchar(500),
	@Sex varchar(10),
	@Address1 varchar(50),
	@Address2 varchar(50),
	@City varchar(20),
	@ZipCode varchar(20),
	@StateCode char(2),
	@AvaterFilename varchar(200)
)

AS

BEGIN
INSERT INTO Accounts
VALUES(@Username, @Password, 1)


INSERT INTO Musician
VALUES(@Fname, @Lname, @DateOfBirth, @Phone, @Email, @Instrument, @Sex,
       @Address1, @Address2, @City, @Zipcode,  @@IDENTITY, @StateCode, 
       @AvaterFilename)
END
GO
USE [master]
GO
ALTER DATABASE [BuildABand] SET  READ_WRITE 
GO