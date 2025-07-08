import { EventTrigger } from "./eventTrigger";
import { Observation } from "./observation";
import { ObservedGrade } from "./observedGrade";

export interface SegFormInputJunction {
    segId? : number,
    position? : number,
    eventTrigger? : EventTrigger,
    observations? : Observation[],
    lGrade? : ObservedGrade[],
    rGrade? : ObservedGrade[]
}