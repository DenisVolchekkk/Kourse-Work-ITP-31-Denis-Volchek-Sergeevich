Use LessonsDB
go
ALTER VIEW Teacher_v
AS SELECT L.TeacherID AS ID,
		  T.TeacherName AS Name,
		  D.DisciplineName AS DisciplineName,
		  DT.DisciplineType AS DisciplineType,
		  L.LessonDate AS LessonDate,
		  LT.LessonTime AS LessonTime
From Lessons AS L 
join Teachers AS T on L.TeacherID = T.TeacherID
join Disciplines AS D on L.DisciplineID = D.DisciplineID 
join DisciplineTypes AS DT on L.DisciplineTypeID = DT.DisciplineTypeID 
join LessonsTime AS LT on L.LessonTimeID = LT.LessonTimeID 
WHERE L.Semestr = '1' and L.Year = '2023' and T.TeacherID = '1'
go
ALTER VIEW Group_v
AS SELECT L.StudentsGroupID As StudentsGroupNumber,
          L.ClassroomID As NumberOfClassroom
From Lessons AS L 
join StudentsGroups AS SG on L.StudentsGroupID = SG.StudentsGroupID
WHERE L.StudentsGroupID = '1'
go
ALTER VIEW Group_v
AS SELECT L.StudentsGroupID As StudentsGroupNumber,
          L.ClassroomID As NumberOfClassroom
From Lessons AS L 
join StudentsGroups AS SG on L.StudentsGroupID = SG.StudentsGroupID
WHERE L.StudentsGroupID = '1'
go
ALTER VIEW Facility_v
AS SELECT L.StudentsGroupID As StudentsGroupNumber,
          L.ClassroomID As NumberOfClassroom,
		  T.TeacherName AS Name,
		  D.DisciplineName AS DisciplineName,
		  DT.DisciplineType AS DisciplineType,
		  L.LessonDate AS LessonDate,
		  LT.LessonTime AS LessonTime
From Lessons AS L 
join Teachers AS T on L.TeacherID = T.TeacherID
join Disciplines AS D on L.DisciplineID = D.DisciplineID 
join DisciplineTypes AS DT on L.DisciplineTypeID = DT.DisciplineTypeID 
join LessonsTime AS LT on L.LessonTimeID = LT.LessonTimeID 
join StudentsGroups AS SG on L.StudentsGroupID = SG.StudentsGroupID
join Facilities AS F on F.FacilityID = SG.FacilityID
WHERE L.StudentsGroupID = '7' and SG.FacilityId = '2' and L.Semestr = '2'
go
SELECT * FROM Facility_v
go
SELECT * FROM Teacher_v
go
Select * From Group_v