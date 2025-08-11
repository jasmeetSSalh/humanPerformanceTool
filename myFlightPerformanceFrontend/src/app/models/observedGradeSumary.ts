export interface ObservedGradeSummary {
    category: Categories;
    pilotEmpId: Number;
    score: Number
}

export enum Categories {
    ProceduralUseAdherence = "Procedural Use & Adherence",
    SituationalAwareness = "Situational Awareness",
    VerficationPractices = "Verification Practices",
    OverallScore = "Overall Score",
    Briefing = "Briefing",
    Communication = "Communication",
    StopWhenUnsure = "Stop When Unsure",
    SelfCheck = "Self Check",
    Conservatism = "Conservatism"
}