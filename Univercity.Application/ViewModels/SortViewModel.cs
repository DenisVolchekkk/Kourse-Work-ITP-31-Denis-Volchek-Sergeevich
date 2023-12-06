namespace Univercity.Application.ViewModels
{
    public enum SortState
    {
        No, 
        DisciplineNameAsc,
        DisciplineNameDesc,    
        ClassroomAsc,
        ClassroomDesc,   
        LessonTimeAsc, 
        LessonTimeDesc,    
        TeacherAsc,
        TeacherDesc,    
        StudentGroupAsc,
        StudentGroupDesc,   
        DisciplineTypeAsc,
        DisciplineTypeDesc,  
        SemestrAsc,
        SemestrDesc,    
        YearAsc,
        YearDesc,   
        DayofWeekAsc,
        DayofWeekDesc,  
        DateAsc,
        DateDesc,   
        FacilityNameAsc,
        FacilityNameDesc    
    }
    public class SortViewModel
    {
        public SortState DisciplineNameSort { get; set; } 
        public SortState LessonTimeSort { get; set; }    
        public SortState TeacherSort { get; set; }
        public SortState ClassroomSort { get; set; }
        public SortState StudentGroupState { get; set; }
        public SortState DisciplineTypeState { get; set; }
        public SortState YearSort { get; set; }
        public SortState SemestrState { get; set; }
        public SortState DayOfWeekState { get; set; }
        public SortState DateState { get; set; }
        public SortState FacilityNameState { get; set; }

        public SortState CurrentState { get; set; }
     

        public SortViewModel(SortState sortOrder)
        {
            DisciplineNameSort = sortOrder == SortState.DisciplineNameAsc ? SortState.DisciplineNameDesc : SortState.DisciplineNameAsc;
            LessonTimeSort = sortOrder == SortState.LessonTimeAsc ? SortState.LessonTimeDesc : SortState.LessonTimeAsc;
            TeacherSort = sortOrder == SortState.TeacherAsc ? SortState.TeacherDesc : SortState.TeacherAsc;
            ClassroomSort = sortOrder == SortState.ClassroomAsc ? SortState.ClassroomDesc : SortState.ClassroomAsc;
            StudentGroupState = sortOrder == SortState.StudentGroupAsc ? SortState.StudentGroupDesc : SortState.StudentGroupAsc;
            DisciplineTypeState = sortOrder == SortState.DisciplineTypeAsc ? SortState.DisciplineTypeDesc : SortState.DisciplineTypeAsc;
            YearSort = sortOrder == SortState.YearAsc ? SortState.YearDesc : SortState.YearAsc;
            SemestrState = sortOrder == SortState.SemestrAsc ? SortState.SemestrDesc : SortState.SemestrAsc;
            DayOfWeekState = sortOrder == SortState.DayofWeekAsc ? SortState.DayofWeekDesc : SortState.DayofWeekAsc;
            DateState = sortOrder == SortState.DateAsc ? SortState.DateDesc : SortState.DateAsc;
            FacilityNameState = sortOrder == SortState.FacilityNameAsc ? SortState.FacilityNameDesc : SortState.FacilityNameAsc;


            CurrentState = sortOrder;
        }



    }
}
