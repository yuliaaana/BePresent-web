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


CREATE TABLE "AspNetRoles" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(256),
    "NormalizedName" VARCHAR(256),
    "ConcurrencyStamp" VARCHAR(256)
);

CREATE TABLE "AspNetUsers" (
    "Id" SERIAL PRIMARY KEY,
    "UserName" VARCHAR(256),
    "NormalizedUserName" VARCHAR(256),
    "Email" VARCHAR(256),
    "NormalizedEmail" VARCHAR(256),
    "EmailConfirmed" BOOLEAN NOT NULL DEFAULT FALSE,
    "PasswordHash" TEXT,
    "SecurityStamp" TEXT,
    "ConcurrencyStamp" TEXT,
    "PhoneNumber" TEXT,
    "PhoneNumberConfirmed" BOOLEAN NOT NULL DEFAULT FALSE,
    "TwoFactorEnabled" BOOLEAN NOT NULL DEFAULT FALSE,
    "LockoutEnd" TIMESTAMP WITH TIME ZONE,
    "LockoutEnabled" BOOLEAN NOT NULL DEFAULT FALSE,
    "AccessFailedCount" INT NOT NULL DEFAULT 0
);

CREATE TABLE "AspNetRoleClaims" (
    "Id" SERIAL PRIMARY KEY,
    "RoleId" INT NOT NULL,
    "ClaimType" TEXT,
    "ClaimValue" TEXT,
    CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserClaims" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" INT NOT NULL,
    "ClaimType" TEXT,
    "ClaimValue" TEXT,
    CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserLogins" (
    "LoginProvider" VARCHAR(128) NOT NULL,
    "ProviderKey" VARCHAR(128) NOT NULL,
    "ProviderDisplayName" TEXT,
    "UserId" INT NOT NULL,
    CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserRoles" (
    "UserId" INT NOT NULL,
    "RoleId" INT NOT NULL,
    CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserTokens" (
    "UserId" INT NOT NULL,
    "LoginProvider" VARCHAR(128) NOT NULL,
    "Name" VARCHAR(128) NOT NULL,
    "Value" TEXT,
    CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

ALTER TABLE "AspNetUsers"
    ADD COLUMN IF NOT EXISTS "DateOfBirth" TIMESTAMP NULL,
    ADD COLUMN IF NOT EXISTS "Gender" VARCHAR(50) NULL,
    ADD COLUMN IF NOT EXISTS "Interests" TEXT[] NULL,
    ADD COLUMN IF NOT EXISTS "IsAuthorized" BOOLEAN NOT NULL DEFAULT FALSE;

    UPDATE "AspNetUsers"
SET "EmailConfirmed" = true
WHERE "NormalizedEmail" = 'YULI2@GMAIL.COM';
