IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Audit')
    CREATE TABLE [Audit](
        [Id] [uniqueidentifier] NOT NULL,
        [CorrelationId] [varchar](255) NULL,
        [ReplyToAddress] [varchar](255) NULL,
        [Recoverable] [bit] NOT NULL,
        [Expires] [datetime] NULL,
        [Headers] [varchar](max) NOT NULL,
        [Body] [varbinary](max) NULL,
        [RowVersion] [bigint] IDENTITY(1,1) NOT NULL
        ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
        GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Error')
    CREATE TABLE [Error](
        [Id] [uniqueidentifier] NOT NULL,
        [CorrelationId] [varchar](255) NULL,
        [ReplyToAddress] [varchar](255) NULL,
        [Recoverable] [bit] NOT NULL,
        [Expires] [datetime] NULL,
        [Headers] [varchar](max) NOT NULL,
        [Body] [varbinary](max) NULL,
        [RowVersion] [bigint] IDENTITY(1,1) NOT NULL
        ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
        GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Subscription')
    CREATE TABLE [Subscription](
        [QueueAddress] [nvarchar](200) NOT NULL,
        [Endpoint] [nvarchar](200) NOT NULL,
        [Topic] [nvarchar](200) NOT NULL,
        PRIMARY KEY CLUSTERED
    (   [Endpoint] ASC,
        [Topic] ASC
        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    ) ON [PRIMARY]
    GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='DataFileProcessorEndpoint')
BEGIN
    CREATE TABLE [DataFileProcessorEndpoint](
        [Id] [uniqueidentifier] NOT NULL,
        [CorrelationId] [varchar](255) NULL,
        [ReplyToAddress] [varchar](255) NULL,
        [Recoverable] [bit] NOT NULL,
        [Expires] [datetime] NULL,
        [Headers] [nvarchar](max) NOT NULL,
        [Body] [varbinary](max) NULL,
        [RowVersion] [bigint] IDENTITY(1,1) NOT NULL
        ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
    
    CREATE TABLE [DataFileProcessorEndpoint.Delayed](
        [Headers] [nvarchar](max) NOT NULL,
        [Body] [varbinary](max) NULL,
        [Due] [datetime] NOT NULL,
        [RowVersion] [bigint] IDENTITY(1,1) NOT NULL
        ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
    
    CREATE NONCLUSTERED INDEX IX_DataFileProcessorEndpoint_RowVersion
    ON DataFileProcessorEndpoint (RowVersion);
    
    CREATE NONCLUSTERED INDEX IX_DataFileProcessorEndpoint_Delayed_RowVersion
    ON [DataFileProcessorEndpoint.Delayed] (RowVersion);

    INSERT INTO [dbo].[Subscription]([QueueAddress] ,[Endpoint] ,[Topic])
    VALUES 
           ('DataFileProcessorEndpoint','DataFileProcessorEndpoint' ,'InsuranceDetails.Messages.UpdateBasicHealthInsuranceCommand'),
           ('DataFileProcessorEndpoint','DataFileProcessorEndpoint' ,'InsuranceDetails.Messages.UpdateSupplementaryHealthInsuranceCommand')
    ;

END