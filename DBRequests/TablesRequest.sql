Use LessonsDB
go
DROP TABLE Lessons,LessonsTime,Disciplines,Classrooms,DisciplineTypes, Teachers,StudentsGroups,Facilities
-- �������� ������� LessonsTime
CREATE TABLE LessonsTime (
    LessonTimeID INT IDENTITY(1,1) PRIMARY KEY,
    LessonTime Time NOT NULL
);
-- �������� ������� Disciplines
CREATE TABLE Disciplines (
    DisciplineID INT IDENTITY(1,1) PRIMARY KEY,
    DisciplineName VARCHAR(50) NOT NULL
);

-- �������� ������� Classroom
CREATE TABLE Classrooms (
    ClassroomID INT IDENTITY(1,1) PRIMARY KEY,
    NumberOfClassroom INT NOT NULL,
	Places INT NOT NULL,
	Wing Int NOT NULL,
	ClassroomType varchar(50) NOT NULL
);
--�������� ������� DisciplineTypes
CREATE TABLE DisciplineTypes (
    DisciplineTypeID INT IDENTITY(1,1) PRIMARY KEY,
	DisciplineType VARCHAR(50) NOT NULL
);
--�������� ������� Teachers
CREATE TABLE Teachers (
    TeacherID INT IDENTITY(1,1) PRIMARY KEY,
	TeacherName VARCHAR(50) NOT NULL
);
--�������� ������� Ficilities
CREATE TABLE Facilities (
    FacilityID INT IDENTITY(1,1) PRIMARY KEY,
	FacilityName VARCHAR(50) NOT NULL
);
--�������� ������� StudentsGroups
CREATE TABLE StudentsGroups (
    StudentsGroupID INT IDENTITY(1,1) PRIMARY KEY,
	NumberOfGroup VARCHAR(50) NOT NULL,
	QuantityOfStudents INT,
	FacilityId INT,
	FOREIGN KEY (FacilityId) REFERENCES Facilities(FacilityId)
);
-- �������� ������� Lessons
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
	FOREIGN KEY (LessonTimeID) REFERENCES LessonsTime(LessonTimeID),
	FOREIGN KEY (ClassroomID) REFERENCES Classrooms(ClassroomID),
	FOREIGN KEY (DisciplineID) REFERENCES Disciplines(DisciplineID),
	FOREIGN KEY (DisciplineTypeID) REFERENCES DisciplineTypes(DisciplineTypeID),
	FOREIGN KEY (TeacherID) REFERENCES Teachers(TeacherID),
	FOREIGN KEY (StudentsGroupID) REFERENCES StudentsGroups(StudentsGroupID)

    
    
);