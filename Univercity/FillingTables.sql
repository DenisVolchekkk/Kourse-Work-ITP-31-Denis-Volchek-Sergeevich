
----Заполнение таблицы LessonsTime
INSERT INTO LessonsTimes (LessonTime) 
Values
	('8:20:00'),
	('10:00:00'),
	('11:35:00'),
	('13:30:00'),
	('15:05:00'),
	('16:45:00'),
	('18:20:00'),
	('20:50:00')

----Заполнение таблицы Teachers

-- Создание временной таблицы с именами
CREATE TABLE #FirstNames (
    id INT IDENTITY(1, 1) PRIMARY KEY,
    firstname VARCHAR(100)
);

-- Вставка 10 записей в таблицу с именами
INSERT INTO #FirstNames (firstname)
VALUES
    ('Денис'),
    ('Игорь'),
    ('Михаил'),
    ('Андрей'),
    ('Владимир'),
    ('Владислав'),
    ('Алексей'),
    ('Кирилл'),
    ('Григорий'),
    ('Иван')

-- Создание временной таблицы с фамилиями
CREATE TABLE #LastNames (
    id INT IDENTITY(1, 1) PRIMARY KEY,
    lastname VARCHAR(100)
);

-- Вставка 10 записей в таблицу с фамилиями
INSERT INTO #LastNames (lastname)
VALUES
    ('Волчек'),
    ('Суворов'),
    ('Сидоров'),
    ('Петров'),
    ('Моиссенко'),
    ('Маслак'),
    ('Бондарев'),
    ('Данилков'),
    ('Иванов'),
    ('Суковатый')

-- Создание временной таблицы с отчествами
CREATE TABLE #MiddleNames (
    id INT IDENTITY(1, 1) PRIMARY KEY,
    middlename VARCHAR(100)
);

-- Вставка 10 записей в таблицу с отчествами
INSERT INTO #MiddleNames (middlename)
VALUES
    ('Денисович'),
    ('Игороревич'),
    ('Михаилович'),
    ('Андревич'),
    ('Владимирович'),
    ('Владиславович'),
    ('Алексеевич'),
    ('Кириллович'),
    ('Григорьевич'),
    ('Иванович')
SET IDENTITY_INSERT Teachers ON;
DECLARE @Counter INT = 1;
DECLARE @lastName varchar(50) 
DECLARE @firstName varchar(50) 
DECLARE @middleName varchar(50) 
WHILE @Counter <= 1000
BEGIN
Select @lastName = #LastNames.lastname From #LastNames Where #LastNames.id = Round(RAND()*(10-1+1)+1, 0)
Select @firstName = #FirstNames.firstname From #FirstNames Where #FirstNames.id = Round(RAND()*(10-1+1)+1, 0)
Select @middleName = #MiddleNames.middlename From #MiddleNames Where #MiddleNames.id = Round(RAND()*(10-1+1)+1, 0)
INSERT INTO Teachers(TeacherID,TeacherName)
	values(@Counter, @lastName + ' ' + @firstName + ' ' + @middleName)
	Set @Counter = @Counter + 1
END
DELETE FROM Teachers
WHERE TeacherID NOT IN (
    SELECT MIN(TeacherID)
    FROM Teachers
    GROUP BY  TeacherName
);
DROP TABLE #FirstNames;
DROP TABLE #LastNames;
DROP TABLE #MiddleNames;

--Заполнение таблицы Facilities
INSERT INTO Facilities(Facilities.FacilityName)
VALUES
	('ФАИС'),
	('ГЭФ'),
	('Машфак'),
	('Энергофак'),
	('МТФ')
--Заполнение таблицы StudentsGroups
set @Counter = 1;
WHILE @Counter <= 250
BEGIN
INSERT INTO StudentsGroups(NumberOfGroup,QuantityOfStudents,FacilityId)
	VALUES
	(@Counter, Round(RAND() * (30 - 1 + 1) + 1,0), Round(RAND() * (4- 1 + 1) + 1,0))
	SET @Counter = @Counter + 1;
END
----Заполнение таблицы Classrooms

CREATE TABLE AuditoryTypes (
    id INT IDENTITY(1, 1) PRIMARY KEY,
    AuditoryType VARCHAR(50)
);
INSERT INTO AuditoryTypes (AuditoryType)
VALUES
    ('Аудитория'),
    ('Кабинет')
DECLARE @auditory varchar(50) 
set @Counter = 1;
WHILE @Counter <= 250
BEGIN
Select @auditory = AuditoryTypes.AuditoryType From AuditoryTypes Where AuditoryTypes.id = Round(RAND()*(2-1+1)+1, 0)
INSERT INTO Classrooms(NumberOfClassroom,Places,Wing,ClassroomType)
	VALUES
	(@Counter, Round(RAND() * (100 - 50 + 1) + 50,0),Round(RAND() * (1 -1 + 1) + 1,0),@auditory)
	SET @Counter = @Counter + 1;
END
DROP TABLE AuditoryTypes

--Заполнение таблицы DisciplineTypes
INSERT INTO DisciplineTypes(TypeOfDiscipline)
VALUES
	('Лекция'),
	('Практика'),
	('Лабораторная работа')
--Заполнение таблицы Disciplines
INSERT INTO Disciplines(DisciplineName)
VALUES
	('ООП'),
	('Математический аниз'),
	('Ангийский язык'),
	('БЖЧ'),
	('Теория вероятности'),
	('Дискретная математик'),
	('Кластерный анализ'),
	('Базы данных'),
	('История'),
	('Философия')

--Заполнение таблицы Lessons
set @Counter  = 1;;
DECLARE @randomDate DATE;
DECLARE @startYear INT = 2023;
DECLARE @startDate DATE = DATEFROMPARTS(@startYear, 1, 1);
DECLARE @endDate DATE = DATEFROMPARTS(@startYear, 12, 31);
DECLARE @totalDays INT = DATEDIFF(DAY, @startDate, @endDate);

WHILE @Counter <= 250
BEGIN
    SET @randomDate = DATEADD(DAY, CAST(RAND() * @totalDays AS INT), @startDate);
    INSERT INTO Lessons (DisciplineID, ClassroomID, DisciplineTypeID, TeacherID, StudentsGroupID, LessonDate, DayOfWeek,Semestr,Year,LessonTimeID)
    VALUES (Round(RAND()*(10-1+1)+1, 0), Round(RAND()*(100-1+1)+1, 0),Round(RAND()*(2-1+1)+1, 0),Round(RAND()*(50-1+1)+1, 0),Round(RAND()*(60-1+1)+1, 0), @randomDate,Round(RAND()*(6-1+1)+1, 0),Round(RAND()+1, 0),Year(@randomDate),Round(RAND()*(9-1+1)+1, 0)  )
            -- Здесь можно указать логику для заполнения остальных столбцов
            -- DisciplineID, ClassroomID, DisciplineTypeID, TeacherID, StudentsGroupID, LessonDate, DayOfWeek);

    SET @Counter = @Counter + 1;
END

DELETE FROM Lessons
WHERE LessonID NOT IN (
    SELECT MIN(LessonID)
    FROM Lessons
    GROUP BY  DisciplineID, ClassroomID, DisciplineTypeID, TeacherID, StudentsGroupID, LessonDate, DayOfweek, Semestr, Year,LessonTimeID
);