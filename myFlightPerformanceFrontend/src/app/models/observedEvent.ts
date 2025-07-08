export interface ObservedEvent {
    
    segId: number;
    flyingPilot: number;
    nonFlyingPilot: number;
    focusArea: string;

    // save parameters used by the backend
    eventId? : number;
    eventCreationDate? : Date;
}