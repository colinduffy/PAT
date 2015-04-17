CREATE TABLE [dbo].[Answer] (
    [AnswerId]   INT            IDENTITY (1, 1) NOT NULL,
    [QuestionID] INT            NOT NULL,
    [Answer]     NVARCHAR (300) NOT NULL,
    [IsCorrect]  BIT            NOT NULL,
    PRIMARY KEY CLUSTERED ([AnswerId] ASC)
);

CREATE TABLE [dbo].[Question] (
    [QuestionId] INT            IDENTITY (1, 1) NOT NULL,
    [TopicID]    INT            NOT NULL,
    [Question]   NVARCHAR (300) NOT NULL,
    PRIMARY KEY CLUSTERED ([QuestionId] ASC)
);


CREATE TABLE [dbo].[Resources] (
    [ResourceId] INT            IDENTITY (1, 1) NOT NULL,
    [TopicId]    INT            NOT NULL,
    [Link]       NVARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([ResourceId] ASC)
);

CREATE TABLE [dbo].[Subject] (
    [SubjectId]   INT           IDENTITY (1, 1) NOT NULL,
    [Description] NVARCHAR (30) NOT NULL,
    PRIMARY KEY CLUSTERED ([SubjectId] ASC)
);

CREATE TABLE [dbo].[TakenTests] (
    [TakenId] INT IDENTITY (1, 1) NOT NULL,
    [UserId]  INT NOT NULL,
    [TestId]  INT NOT NULL,
	[Score] INT NOT NULL,
	[Passed] BIT NOT NULL,
    PRIMARY KEY CLUSTERED ([TakenId] ASC)
);

CREATE TABLE [dbo].[Test] (
    [TestId]      INT            IDENTITY (1, 1) NOT NULL,
    [Level]       INT            NOT NULL,
    [Description] NVARCHAR (300) NOT NULL,
    [SubjectId]   INT            NOT NULL,
    [Threshold]   INT            DEFAULT ((75)) NOT NULL,
    PRIMARY KEY CLUSTERED ([TestId] ASC)
);

CREATE TABLE [dbo].[TestTopics] (
    [Id]            INT IDENTITY (1, 1) NOT NULL,
    [TopicId]       INT NOT NULL,
    [TestId]        INT NOT NULL,
    [Quantity]      INT NOT NULL,
    [QuestionValue] INT DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


CREATE TABLE [dbo].[Topics] (
    [TopicId]     INT            IDENTITY (1, 1) NOT NULL,
    [description] NCHAR (30)     NOT NULL,
    [Message]     NVARCHAR (MAX) DEFAULT ('') NOT NULL,
    PRIMARY KEY CLUSTERED ([TopicId] ASC)
);

CREATE TABLE [dbo].[AnsweredQuestions]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,
	[takenId] INT NOT NULL,-- REFERENCES [dbo].[TakenTests],
	[questionId] INT NOT NULL,-- REFERENCES [dbo].[Question],
	[choice] INT NOT NULL,--- REFERENCES [dbo].[Answer],
	PRIMARY KEY CLUSTERED ([Id] ASC)
)


CREATE TABLE [dbo].[UserProfile] (
    [UserId]    INT            IDENTITY (1, 1) NOT NULL,
    [UserName]  NVARCHAR (MAX) NULL,
    [FirstName] NVARCHAR (MAX) NULL,
    [LastName]  NVARCHAR (MAX) NULL,
    [StudentID] NVARCHAR (50)  NULL,
    [Phone]     NVARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC)
);

CREATE TABLE [dbo].[webpages_Membership] (
    [UserId]                                  INT            NOT NULL,
    [CreateDate]                              DATETIME       NULL,
    [ConfirmationToken]                       NVARCHAR (128) NULL,
    [IsConfirmed]                             BIT            DEFAULT ((0)) NULL,
    [LastPasswordFailureDate]                 DATETIME       NULL,
    [PasswordFailuresSinceLastSuccess]        INT            DEFAULT ((0)) NOT NULL,
    [Password]                                NVARCHAR (128) NOT NULL,
    [PasswordChangedDate]                     DATETIME       NULL,
    [PasswordSalt]                            NVARCHAR (128) NOT NULL,
    [PasswordVerificationToken]               NVARCHAR (128) NULL,
    [PasswordVerificationTokenExpirationDate] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC)
);

CREATE TABLE [dbo].[webpages_OAuthMembership] (
    [Provider]       NVARCHAR (30)  NOT NULL,
    [ProviderUserId] NVARCHAR (100) NOT NULL,
    [UserId]         INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Provider] ASC, [ProviderUserId] ASC)
);

CREATE TABLE [dbo].[webpages_Roles] (
    [RoleId]   INT            IDENTITY (1, 1) NOT NULL,
    [RoleName] NVARCHAR (256) NOT NULL,
    PRIMARY KEY CLUSTERED ([RoleId] ASC),
    UNIQUE NONCLUSTERED ([RoleName] ASC)
);

CREATE TABLE [dbo].[webpages_UsersInRoles] (
    [UserId] INT NOT NULL,
    [RoleId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
    CONSTRAINT [fk_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[UserProfile] ([UserId]),
    CONSTRAINT [fk_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[webpages_Roles] ([RoleId])
);

