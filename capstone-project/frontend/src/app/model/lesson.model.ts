import { Material } from './material.model';

export class Lesson {
    constructor(
        public id: number,
        public title: string,
        public description: string,
        public courseId: number,
        public createdAt: Date,
        public sequenceNo: number,
        public materials: { $id: string; $values: Material[] }
    ){}
}
