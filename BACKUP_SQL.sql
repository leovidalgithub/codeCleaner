USE [DBCleaningBackup]
GO
/****** Object:  User [datatail02]    Script Date: 4/15/2019 1:56:24 PM ******/
CREATE USER [datatail02] FOR LOGIN [datatail02] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [datatail02]
GO
/****** Object:  UserDefinedTableType [dbo].[LogDataType]    Script Date: 4/15/2019 1:56:25 PM ******/
CREATE TYPE [dbo].[LogDataType] AS TABLE(
	[DateOfTheRun] [datetime] NOT NULL,
	[ThisMachineName] [varchar](50) NOT NULL,
	[SearchRootFolder] [varchar](max) NOT NULL,
	[RepositoryName] [varchar](max) NOT NULL,
	[FilesCountCurrent] [int] NOT NULL,
	[FilesCountPrevious] [int] NOT NULL,
	[FilesCountNew] [int] NOT NULL,
	[FilesCountChanged] [int] NOT NULL,
	[FilesCountMissing] [int] NOT NULL,
	[ElapsedTimeCurrent] [bigint] NOT NULL,
	[ElapsedTimePrevious] [bigint] NOT NULL,
	[ElapsedTimeCompare] [bigint] NOT NULL,
	[ElapsedTimeSaving] [bigint] NOT NULL,
	[ElapsedTimeOverall] [bigint] NOT NULL,
	[FinalStepOKay] [bit] NOT NULL,
	[ErrorDetected] [bit] NOT NULL,
	[ErrorFSutilBehavior] [varchar](max) NULL,
	[ErrorGettingRepoInfo] [varchar](max) NULL,
	[ErrorGettingCurrent] [varchar](max) NULL,
	[ErrorGettingPrevious] [varchar](max) NULL,
	[ErrorComparing] [varchar](max) NULL,
	[ErrorSaving] [varchar](max) NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[RepositoryFiles]    Script Date: 4/15/2019 1:56:25 PM ******/
CREATE TYPE [dbo].[RepositoryFiles] AS TABLE(
	[CodeCleanerInfoID] [int] NOT NULL,
	[Path] [varchar](300) NOT NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
	[Accessed] [datetime] NOT NULL,
	[Size] [decimal](20, 2) NULL,
	[Changes] [int] NOT NULL,
	[Active] [bit] NOT NULL
)
GO
/****** Object:  Table [dbo].[CodeCleanerContent]    Script Date: 4/15/2019 1:56:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CodeCleanerContent](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CodeCleanerInfoID] [int] NOT NULL,
	[Path] [varchar](max) NOT NULL,
	[tracingFromDate] [datetime] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
	[Accessed] [datetime] NOT NULL,
	[Size] [bigint] NOT NULL,
	[Changes] [int] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_CodeCleanerContent] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CodeCleanerInfo]    Script Date: 4/15/2019 1:56:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CodeCleanerInfo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MachineName] [varchar](50) NOT NULL,
	[SearchRootFolder] [varchar](150) NOT NULL,
	[RepositoryName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_CodeCleanerInfo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CodeCleanerLog]    Script Date: 4/15/2019 1:56:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CodeCleanerLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DateOfTheRun] [datetime] NOT NULL,
	[ThisMachineName] [varchar](50) NOT NULL,
	[SearchRootFolder] [varchar](max) NOT NULL,
	[RepositoryName] [varchar](max) NOT NULL,
	[FilesCountCurrent] [int] NOT NULL,
	[FilesCountPrevious] [int] NOT NULL,
	[FilesCountNew] [int] NOT NULL,
	[FilesCountChanged] [int] NOT NULL,
	[FilesCountMissing] [int] NOT NULL,
	[ElapsedTimeCurrent] [bigint] NOT NULL,
	[ElapsedTimePrevious] [bigint] NOT NULL,
	[ElapsedTimeCompare] [bigint] NOT NULL,
	[ElapsedTimeSaving] [bigint] NOT NULL,
	[ElapsedTimeOverall] [bigint] NOT NULL,
	[ErrorDetected] [bit] NOT NULL,
	[ErrorFSutilBehavior] [varchar](max) NULL,
	[ErrorGettingRepoInfo] [varchar](max) NULL,
	[ErrorGettingCurrent] [varchar](max) NULL,
	[ErrorGettingPrevious] [varchar](max) NULL,
	[ErrorComparing] [varchar](max) NULL,
	[ErrorSaving] [varchar](max) NULL,
	[FinalStepOKay] [bit] NOT NULL,
 CONSTRAINT [PK_CodeCleanerLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[CodeCleanerContent] ADD  CONSTRAINT [DF_CodeCleanerContent_tracingFromDate]  DEFAULT (getdate()) FOR [tracingFromDate]
GO
ALTER TABLE [dbo].[CodeCleanerContent] ADD  CONSTRAINT [DF_CodeCleanerContent_Size]  DEFAULT ((0)) FOR [Size]
GO
ALTER TABLE [dbo].[CodeCleanerContent] ADD  CONSTRAINT [DF_CodeCleanerContent_changes]  DEFAULT ((0)) FOR [Changes]
GO
ALTER TABLE [dbo].[CodeCleanerContent] ADD  CONSTRAINT [DF_CodeCleanerContent_active]  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [dbo].[CodeCleanerLog] ADD  CONSTRAINT [DF_CodeCleanerLog_FilesCountCurrent]  DEFAULT ((0)) FOR [FilesCountCurrent]
GO
ALTER TABLE [dbo].[CodeCleanerLog] ADD  CONSTRAINT [DF_CodeCleanerLog_FilesCountPrevious]  DEFAULT ((0)) FOR [FilesCountPrevious]
GO
ALTER TABLE [dbo].[CodeCleanerLog] ADD  CONSTRAINT [DF_CodeCleanerLog_FilesCountNew]  DEFAULT ((0)) FOR [FilesCountNew]
GO
ALTER TABLE [dbo].[CodeCleanerLog] ADD  CONSTRAINT [DF_CodeCleanerLog_FilesCountChanged]  DEFAULT ((0)) FOR [FilesCountChanged]
GO
ALTER TABLE [dbo].[CodeCleanerLog] ADD  CONSTRAINT [DF_CodeCleanerLog_FilesCountMissing]  DEFAULT ((0)) FOR [FilesCountMissing]
GO
ALTER TABLE [dbo].[CodeCleanerLog] ADD  CONSTRAINT [DF_CodeCleanerLog_ElapsedTimeCurrent]  DEFAULT ((0)) FOR [ElapsedTimeCurrent]
GO
ALTER TABLE [dbo].[CodeCleanerLog] ADD  CONSTRAINT [DF_CodeCleanerLog_ElapsedTimePrevious]  DEFAULT ((0)) FOR [ElapsedTimePrevious]
GO
ALTER TABLE [dbo].[CodeCleanerLog] ADD  CONSTRAINT [DF_CodeCleanerLog_ElapsedTimeCompare]  DEFAULT ((0)) FOR [ElapsedTimeCompare]
GO
ALTER TABLE [dbo].[CodeCleanerLog] ADD  CONSTRAINT [DF_CodeCleanerLog_ElapsedTimeSaving]  DEFAULT ((0)) FOR [ElapsedTimeSaving]
GO
ALTER TABLE [dbo].[CodeCleanerLog] ADD  CONSTRAINT [DF_CodeCleanerLog_ElapsedTimeOverall]  DEFAULT ((0)) FOR [ElapsedTimeOverall]
GO
ALTER TABLE [dbo].[CodeCleanerLog] ADD  CONSTRAINT [DF_CodeCleanerLog_ErrorDetected]  DEFAULT ((0)) FOR [ErrorDetected]
GO
ALTER TABLE [dbo].[CodeCleanerLog] ADD  CONSTRAINT [DF_CodeCleanerLog_FinalStepOKay]  DEFAULT ((0)) FOR [FinalStepOKay]
GO
ALTER TABLE [dbo].[CodeCleanerContent]  WITH CHECK ADD  CONSTRAINT [FK_CodeCleanerContent_CodeCleanerInfo] FOREIGN KEY([CodeCleanerInfoID])
REFERENCES [dbo].[CodeCleanerInfo] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CodeCleanerContent] CHECK CONSTRAINT [FK_CodeCleanerContent_CodeCleanerInfo]
GO
/****** Object:  StoredProcedure [dbo].[Cleanup_LogInfo]    Script Date: 4/15/2019 1:56:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Cleanup_LogInfo]
@TrackedFiles LogDataType Readonly
AS
BEGIN

INSERT INTO CodeCleanerLog
           (
				[DateOfTheRun],
				[ThisMachineName],
				[SearchRootFolder],
				[RepositoryName],
				[FilesCountCurrent],
				[FilesCountPrevious],
				[FilesCountNew],
				[FilesCountChanged],
				[FilesCountMissing],
				[ElapsedTimeCurrent],
				[ElapsedTimePrevious],
				[ElapsedTimeCompare],
				[ElapsedTimeSaving],
				[ElapsedTimeOverall],
				[FinalStepOKay],
				[ErrorDetected],
				[ErrorFSutilBehavior],
				[ErrorGettingRepoInfo],
				[ErrorGettingCurrent],
				[ErrorGettingPrevious],
				[ErrorComparing],
				[ErrorSaving]				
		   )
		   SELECT 
		   [DateOfTheRun],
				[ThisMachineName],
				[SearchRootFolder],
				[RepositoryName],
				[FilesCountCurrent],
				[FilesCountPrevious],
				[FilesCountNew],
				[FilesCountChanged],
				[FilesCountMissing],
				[ElapsedTimeCurrent],
				[ElapsedTimePrevious],
				[ElapsedTimeCompare],
				[ElapsedTimeSaving],
				[ElapsedTimeOverall],
				[FinalStepOKay],
				[ErrorDetected],
				[ErrorFSutilBehavior],
				[ErrorGettingRepoInfo],
				[ErrorGettingCurrent],
				[ErrorGettingPrevious],
				[ErrorComparing],
				[ErrorSaving]
			FROM @TrackedFiles;
END
GO
/****** Object:  StoredProcedure [dbo].[Cleanup_TrackFileChanges]    Script Date: 4/15/2019 1:56:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Cleanup_TrackFileChanges]
@TrackedFiles RepositoryFiles Readonly
AS
BEGIN

	MERGE CodeCleanerContent AS TARGET
	USING @TrackedFiles AS SOURCE 
	ON (TARGET.Path = SOURCE.Path)
	WHEN MATCHED 
	THEN
	UPDATE SET target.[CodeCleanerInfoID] =source.[CodeCleanerInfoID]
,target.[Path]				 =source.[Path]           
,target.[Created]			 =source.[Created]        
,target.[Modified]			 =source.[Modified]       
,target.[Accessed]			 =source.[Accessed]       
,target.[Size]				 =source.[Size]           
,target.[Changes]			 =source.[Changes]        
,target.[Active]			 =source.[Active]      

	WHEN NOT MATCHED BY TARGET THEN INSERT 
           ([CodeCleanerInfoID]
		   ,[Path]
           ,[Created]
           ,[Modified]
           ,[Accessed]
           ,[Size]
           ,[Changes]
           ,[Active])
     VALUES
           (source.[CodeCleanerInfoID]
			,source.[Path]           
			,source.[Created]        
			,source.[Modified]       
			,source.[Accessed]       
			,source.[Size]           
			,source.[Changes]        
			,source.[Active]);
END
GO
