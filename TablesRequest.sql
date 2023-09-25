USE LessonsDB
go
DROP TABLE Lessons,Disciplines,Classrooms,DisciplineTypes,Teachers,StudentsGroups,Facilities
-- Создание таблицы Disciplines
CREATE TABLE Disciplines (
    DisciplineID INT PRIMARY KEY,
    DisciplineName VARCHAR(50) NOT NULL
);

-- Создание таблицы Classroom
CREATE TABLE Classrooms (
    ClassroomID INT PRIMARY KEY,
    NumberOfClassroom INT NOT NULL,
	Places INT NOT NULL
);
--Создание таблицы DisciplineTypes
CREATE TABLE DisciplineTypes (
    DisciplineTypeID INT PRIMARY KEY,
	DisciplineType VARCHAR(50) NOT NULL
);
--Создание таблицы Teachers
CREATE TABLE Teachers (
    TeacherID INT PRIMARY KEY,
	TeacherName VARCHAR(50) NOT NULL
);
--Создание таблицы Ficilities
CREATE TABLE Facilities (
    FacilityID INT PRIMARY KEY,
	FacilityName VARCHAR(50) NOT NULL
);
--Создание таблицы StudentsGroups
CREATE TABLE StudentsGroups (
    StudentsGroupID INT PRIMARY KEY,
	NumberOfGroup VARCHAR(50) NOT NULL,
	QuantityOfStudents INT,
	FacilityID INT,
	FOREIGN KEY (FacilityID) REFERENCES Facilities(FacilityID),
);
-- Создание таблицы Lessons
CREATE TABLE Lessons (
    LessonID INT PRIMARY KEY,
    DisciplineID INT,
    ClassroomID INT,
	DisciplineTypeID INT,
	TeacherID INT,
	StudentsGroupID INT,
	LessonDate Date,
	DayOfweek INT,
	FOREIGN KEY (ClassroomID) REFERENCES Classrooms(ClassroomID),
	FOREIGN KEY (DisciplineID) REFERENCES Disciplines(DisciplineID),
	FOREIGN KEY (DisciplineTypeID) REFERENCES DisciplineTypes(DisciplineTypeID),
	FOREIGN KEY (TeacherID) REFERENCES Teachers(TeacherID),
	FOREIGN KEY (StudentsGroupID) REFERENCES StudentsGroups(StudentsGroupID),

    
    
);