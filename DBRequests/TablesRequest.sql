Use LessonsDB
go
DROP TABLE Lessons,LessonsTimes,Disciplines,Classrooms,DisciplineTypes, Teachers,StudentsGroups,Facilities
-- Создание таблицы LessonsTime
CREATE TABLE LessonsTimes (
    LessonTimeID INT IDENTITY(1,1) PRIMARY KEY,
    LessonTime Time NOT NULL
);
-- Создание таблицы Disciplines
CREATE TABLE Disciplines (
    DisciplineID INT IDENTITY(1,1) PRIMARY KEY,
    DisciplineName VARCHAR(50) NOT NULL
);

-- Создание таблицы Classroom
CREATE TABLE Classrooms (
    ClassroomID INT IDENTITY(1,1) PRIMARY KEY,
    NumberOfClassroom INT NOT NULL,
	Places INT NOT NULL,
	Wing Int NOT NULL,
	ClassroomType varchar(50) NOT NULL
);
--Создание таблицы DisciplineTypes
CREATE TABLE DisciplineTypes (
    DisciplineTypeID INT IDENTITY(1,1) PRIMARY KEY,
	TypeOfDiscipline VARCHAR(50) NOT NULL
);
--Создание таблицы Teachers
CREATE TABLE Teachers (
    TeacherID INT IDENTITY(1,1) PRIMARY KEY,
	TeacherName VARCHAR(100) NOT NULL
);
--Создание таблицы Ficilities
CREATE TABLE Facilities (
    FacilityID INT IDENTITY(1,1) PRIMARY KEY,
	FacilityName VARCHAR(100) NOT NULL
);
--Создание таблицы StudentsGroups
CREATE TABLE StudentsGroups (
    StudentsGroupID INT IDENTITY(1,1) PRIMARY KEY,
	NumberOfGroup VARCHAR(50) NOT NULL,
	QuantityOfStudents INT,
	FacilityId INT,
	FOREIGN KEY (FacilityId) REFERENCES Facilities(FacilityId)
);
-- Создание таблицы Lessons
CREATE TABLE Lessons (
    LessonID INT IDENTITY(1,1) PRIMARY KEY,
    DisciplineID INT,
    ClassroomID INT,
	DisciplineTypeID INT,
	TeacherID INT,
	StudentsGroupID INT,
	Semestr INT,
	LessonDate Date,
	LessonTimeID Int,
	Year INT,
	DayOfweek INT,
	FOREIGN KEY (LessonTimeID) REFERENCES LessonsTimes(LessonTimeID),
	FOREIGN KEY (ClassroomID) REFERENCES Classrooms(ClassroomID),
	FOREIGN KEY (DisciplineID) REFERENCES Disciplines(DisciplineID),
	FOREIGN KEY (DisciplineTypeID) REFERENCES DisciplineTypes(DisciplineTypeID),
	FOREIGN KEY (TeacherID) REFERENCES Teachers(TeacherID),
	FOREIGN KEY (StudentsGroupID) REFERENCES StudentsGroups(StudentsGroupID)

    
    
);