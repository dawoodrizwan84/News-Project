CREATE TABLE [dbo].[Articles] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [DateStamp]      DATETIME2 (7)  NOT NULL,
    [LinkText]       NVARCHAR (MAX) NOT NULL,
    [Headline]       NVARCHAR (MAX) NOT NULL,
    [ContentSummary] NVARCHAR (MAX) NOT NULL,
    [Content]        NVARCHAR (MAX) NOT NULL,
    [ImageLink]      NVARCHAR (MAX) NOT NULL,
    [CategoryId]     INT            NOT NULL,
    [Author] INT NOT NULL, 
    CONSTRAINT [PK_Articles] PRIMARY KEY CLUSTERED ([Id] ASC)
);

