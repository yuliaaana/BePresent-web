-- Таблиця Користувачів
CREATE TABLE Users (
    UserId SERIAL PRIMARY KEY,
    Username VARCHAR(50) NOT NULL UNIQUE,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Password VARCHAR(255) NOT NULL,
    DateOfBirth DATE,
    Gender VARCHAR(10),
    Interests TEXT[],
    IsAuthorized BOOLEAN DEFAULT FALSE
);

-- Таблиця Дошок Подарунків
CREATE TABLE GiftBoards (
    BoardId SERIAL PRIMARY KEY,
    UserId INT REFERENCES Users(UserId) ON DELETE CASCADE,
    Name VARCHAR(100) NOT NULL,
    CelebrationDate DATE,
    Collaborators INT[],  -- Тип даних для списку ідентифікаторів співпрацівників
    AccessLevel VARCHAR(10) CHECK (AccessLevel IN ('public', 'friends_only', 'private')),
    Description TEXT,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Таблиця Подарунків
CREATE TABLE Gifts (
    GiftId SERIAL PRIMARY KEY,
    BoardId INT REFERENCES GiftBoards(BoardId) ON DELETE CASCADE,
    Name VARCHAR(100) NOT NULL,
    Description TEXT,
    Link VARCHAR(255),
    ImageUrl VARCHAR(255),
    IsReserved BOOLEAN DEFAULT FALSE,
    ReservedBy INT REFERENCES Users(UserId) ON DELETE SET NULL,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Таблиця Резервувань Подарунків
CREATE TABLE GiftReservations (
    ReservationId SERIAL PRIMARY KEY,
    GiftId INT REFERENCES Gifts(GiftId) ON DELETE CASCADE,
    UserId INT REFERENCES Users(UserId) ON DELETE CASCADE,
    ReservedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(GiftId, UserId)  -- Забезпечує, що один подарунок може бути зарезервований лише одним користувачем
);

-- Таблиця Журналу Дій для Аудиту
CREATE TABLE ActionLogs (
    LogId SERIAL PRIMARY KEY,
    UserId INT REFERENCES Users(UserId) ON DELETE SET NULL,
    Action VARCHAR(255),
    ActionTime TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Таблиця для Підтвердження Реєстрації Користувачів
CREATE TABLE EmailConfirmations (
    ConfirmationId SERIAL PRIMARY KEY,
    UserId INT REFERENCES Users(UserId) ON DELETE CASCADE,
    ConfirmationToken VARCHAR(255) NOT NULL,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    ExpiresAt TIMESTAMP NOT NULL
);
