IF OBJECT_ID('dbo.Events', 'U') IS NULL
BEGIN
    CREATE TABLE Events (
        PostId INT IDENTITY(1,1) PRIMARY KEY,
        Title NVARCHAR(200) NOT NULL,
        Description NVARCHAR(MAX) NOT NULL,
        AddDate DATETIME2 NOT NULL,
        Place NVARCHAR(200) NOT NULL,
        Author NVARCHAR(200) NOT NULL,
        Link NVARCHAR(500) NOT NULL,
        EventDate DATETIME2 NOT NULL
    );
END
GO

IF OBJECT_ID('dbo.Gastro', 'U') IS NULL
BEGIN
    CREATE TABLE Gastro (
        PostId INT IDENTITY(1,1) PRIMARY KEY,
        Title NVARCHAR(200) NOT NULL,
        Description NVARCHAR(MAX) NOT NULL,
        AddDate DATETIME2 NOT NULL,
        Place NVARCHAR(200) NOT NULL,
        Author NVARCHAR(200) NOT NULL,
        Link NVARCHAR(500) NOT NULL,
        Day DATETIME2 NOT NULL
    );
END
GO

IF OBJECT_ID('dbo.Tag_Event', 'U') IS NULL
BEGIN
    CREATE TABLE Tag_Event (
        TagID INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(200) NOT NULL,
        Description NVARCHAR(MAX) NULL
    );
END
GO

IF OBJECT_ID('dbo.Tag_Gastro', 'U') IS NULL
BEGIN
    CREATE TABLE Tag_Gastro (
        TagID INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(200) NOT NULL,
        Description NVARCHAR(MAX) NULL
    );
END
GO

IF OBJECT_ID('dbo.Event_Tag_Join', 'U') IS NULL
BEGIN
    CREATE TABLE Event_Tag_Join (
        EventId INT NOT NULL,
        TagId INT NOT NULL,
        PRIMARY KEY (EventId, TagId),
        FOREIGN KEY (EventId) REFERENCES Events(PostId) ON DELETE CASCADE,
        FOREIGN KEY (TagId) REFERENCES Tag_Event(TagID) ON DELETE CASCADE
    );
END
GO

IF OBJECT_ID('dbo.Gastro_Tag_Join', 'U') IS NULL
BEGIN
    CREATE TABLE Gastro_Tag_Join (
        GastroId INT NOT NULL,
        TagId INT NOT NULL,
        PRIMARY KEY (GastroId, TagId),
        FOREIGN KEY (GastroId) REFERENCES Gastro(PostId) ON DELETE CASCADE,
        FOREIGN KEY (TagId) REFERENCES Tag_Gastro(TagID) ON DELETE CASCADE
    );
END
GO