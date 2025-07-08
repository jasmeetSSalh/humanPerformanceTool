export interface ObservedGrade {
    obsId: number;
    pilotEmpId: number;
    grade: GradeValue;
}

export enum GradeValue {
    NegativeOne = -1,
    Zero = 0,
    One = 1
}

// Factory function to provide default values
export function createObservedGrade(
    obsId: number, 
    pilotEmpId: number, 
    grade: GradeValue = GradeValue.Zero
): ObservedGrade {
    return { obsId, pilotEmpId, grade };
}