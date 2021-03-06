USE [master]
GO
/****** Object:  Database [FundRaising]    Script Date: 10/19/2015 13:31:44 ******/
CREATE DATABASE [FundRaising] ON  PRIMARY 
( NAME = N'FundRaising', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\FundRaising.mdf' , SIZE = 2304KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'FundRaising_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\FundRaising_log.LDF' , SIZE = 504KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [FundRaising] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FundRaising].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [FundRaising] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [FundRaising] SET ANSI_NULLS OFF
GO
ALTER DATABASE [FundRaising] SET ANSI_PADDING OFF
GO
ALTER DATABASE [FundRaising] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [FundRaising] SET ARITHABORT OFF
GO
ALTER DATABASE [FundRaising] SET AUTO_CLOSE ON
GO
ALTER DATABASE [FundRaising] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [FundRaising] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [FundRaising] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [FundRaising] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [FundRaising] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [FundRaising] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [FundRaising] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [FundRaising] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [FundRaising] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [FundRaising] SET  ENABLE_BROKER
GO
ALTER DATABASE [FundRaising] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [FundRaising] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [FundRaising] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [FundRaising] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [FundRaising] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [FundRaising] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [FundRaising] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [FundRaising] SET  READ_WRITE
GO
ALTER DATABASE [FundRaising] SET RECOVERY SIMPLE
GO
ALTER DATABASE [FundRaising] SET  MULTI_USER
GO
ALTER DATABASE [FundRaising] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [FundRaising] SET DB_CHAINING OFF
GO
USE [FundRaising]
GO
/****** Object:  Table [dbo].[Organizations]    Script Date: 10/19/2015 13:31:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Organizations](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Distributor] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NOT NULL,
	[ContactName] [nvarchar](max) NULL,
	[Address1] [nvarchar](max) NULL,
	[Address2] [nvarchar](max) NULL,
	[City] [nvarchar](max) NULL,
	[State] [nvarchar](max) NULL,
	[Postal] [nvarchar](max) NULL,
	[Country] [nvarchar](max) NULL,
	[Phone] [nvarchar](max) NULL,
	[WelcomeMessage] [nvarchar](1000) NULL,
	[Catalog] [nvarchar](max) NULL,
	[ShipToSchool] [bit] NOT NULL,
	[ShioToSchoolOnly] [bit] NOT NULL,
	[ShipToSchoolCatalog] [nvarchar](max) NULL,
	[LoginID] [nvarchar](max) NULL,
	[Password] [nvarchar](max) NULL,
	[ParticipantOption] [nvarchar](max) NULL,
	[PricingLevel] [nvarchar](max) NULL,
	[FreeShippingAmount] [decimal](18, 2) NOT NULL,
	[AutoAssignParticipantID] [bit] NOT NULL,
	[CollectTeacherGrade] [bit] NOT NULL,
	[CollectCellPhone] [bit] NOT NULL,
	[CellPhoneRequired] [bit] NOT NULL,
	[GoalType] [nvarchar](max) NULL,
	[DefaultGoal] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_dbo.Organizations] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Campaign]    Script Date: 10/19/2015 13:31:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Campaign](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OrganizatonID] [int] NULL,
	[CampaignName] [nvarchar](200) NULL,
	[CampaignStartDate] [datetime] NULL,
	[CampaignEndDate] [datetime] NULL
) ON [PRIMARY]
GO
