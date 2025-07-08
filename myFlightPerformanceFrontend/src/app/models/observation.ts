export interface Observation {
    obsId: number;
    subCategory: string; 
    category: string;
    behaviour: string; 

    // Optional foreign key references
    foreign_key_mal_id?: number;
    foreign_key_pro_id?: number;
    foreign_key_other_id?: number;
}
