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

IF OBJECT_ID('dbo.Tag', 'U') IS NULL
BEGIN
    CREATE TABLE Tag (
        TagID INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(200) NOT NULL UNIQUE
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
        FOREIGN KEY (TagId) REFERENCES Tag(TagID) ON DELETE CASCADE
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
        FOREIGN KEY (TagId) REFERENCES Tag(TagID) ON DELETE CASCADE
    );
END
GO

-- Seed unified Tag table
IF NOT EXISTS (SELECT 1 FROM Tag WHERE Name = 'Music')
BEGIN
    INSERT INTO Tag (Name) VALUES
        ('Music'),
        ('Sport'),
        ('Culture'),
        ('Family'),
        ('Outdoor'),
        ('Education'),
        ('Art'),
        ('Technology'),
        ('Pizza'),
        ('Vegan'),
        ('Asian'),
        ('Italian'),
        ('Dessert'),
        ('Healthy'),
        ('FastFood'),
        ('Seafood');
END
GO

-- Seed Events
IF NOT EXISTS (SELECT 1 FROM Events WHERE Title = 'Rock Festival')
BEGIN
    SET IDENTITY_INSERT Events ON;
    
    INSERT INTO Events (PostId, Title, Description, AddDate, Place, Author, Link, EventDate) VALUES
        (1, 'Rock Festival', N'Największy festiwal rockowy w mieście.', '2025-01-01', N'Białystok Arena', 'Admin', 'https://event.com/rock', '2025-02-01'),
        (2, N'Mecz siatkówki', N'Turniej siatkówki amatorskiej.', '2025-01-02', N'Hala Sportowa', 'Admin', 'https://event.com/sport', '2025-02-05'),
        (3, N'Wystawa sztuki współczesnej', N'Prezentacja dzieł lokalnych artystów.', '2025-01-03', N'Galeria Arsenał', 'Curator', 'https://event.com/art', '2025-02-10'),
        (4, N'Piknik rodzinny w parku', N'Dzień pełen zabaw dla całej rodziny.', '2025-01-04', 'Park Planty', 'Admin', 'https://event.com/family', '2025-02-15'),
        (5, 'Tech Conference 2025', N'Konferencja dla programistów i entuzjastów technologii.', '2025-01-05', 'Centrum Konferencyjne', 'TechOrg', 'https://event.com/tech', '2025-02-20');
    
    SET IDENTITY_INSERT Events OFF;
END
GO

-- Seed Gastro
IF NOT EXISTS (SELECT 1 FROM Gastro WHERE Title = 'Pizza Day')
BEGIN
    SET IDENTITY_INSERT Gastro ON;
    
    INSERT INTO Gastro (PostId, Title, Description, AddDate, Place, Author, Link, Day) VALUES
        (6, 'Pizza Day', N'Promocje na pizzę w całym mieście.', '2025-01-03', 'PizzaHouse', 'Admin', 'https://gastro.com/pizza', '2025-01-10'),
        (7, 'Vegan Fest', N'Święto kuchni wegańskiej.', '2025-01-04', 'GreenFood', 'Admin', 'https://gastro.com/vegan', '2025-01-12'),
        (8, 'Asian Food Week', N'Tydzień kuchni azjatyckiej z degustacjami.', '2025-01-05', 'Asia Restaurant', 'Chef Wang', 'https://gastro.com/asian', '2025-01-15'),
        (9, 'Seafood Fiesta', N'Świeże owoce morza prosto z wybrzeża.', '2025-01-06', 'Ocean Bistro', 'Admin', 'https://gastro.com/seafood', '2025-01-18'),
        (10, 'Dessert Paradise', N'Najlepsze desery w mieście w jednym miejscu.', '2025-01-07', 'Sweet Corner', 'Pastry Chef', 'https://gastro.com/dessert', '2025-01-20');
    
    SET IDENTITY_INSERT Gastro OFF;
END
GO

-- Seed Event-Tag associations
IF NOT EXISTS (SELECT 1 FROM Event_Tag_Join WHERE EventId = 1 AND TagId = 1)
BEGIN
    INSERT INTO Event_Tag_Join (EventId, TagId) VALUES
        (1, 1),  -- Rock Festival - Music (TagID 1)
        (2, 2),  -- Mecz siatkówki - Sport (TagID 2)
        (3, 7),  -- Wystawa sztuki - Art (TagID 7)
        (4, 4),  -- Piknik rodzinny - Family (TagID 4)
        (5, 8);  -- Tech Conference - Technology (TagID 8)
END
GO

-- Seed Gastro-Tag associations
IF NOT EXISTS (SELECT 1 FROM Gastro_Tag_Join WHERE GastroId = 6 AND TagId = 9)
BEGIN
    INSERT INTO Gastro_Tag_Join (GastroId, TagId) VALUES
        (6, 9),  -- Pizza Day - Pizza (TagID 9)
        (7, 10), -- Vegan Fest - Vegan (TagID 10)
        (8, 11), -- Asian Food Week - Asian (TagID 11)
        (9, 16), -- Seafood Fiesta - Seafood (TagID 16)
        (10, 13); -- Dessert Paradise - Dessert (TagID 13)
END
GOGO