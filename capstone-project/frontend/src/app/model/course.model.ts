import { Lesson } from "./lesson.model";

export class Course {
  constructor(
    public id: number,
    public title: string,
    public description: string,
    public createdById: string,
    public createdAt: Date,
    public thumbnail: string,
    public lessonsResponse: { $id: string; $values: Lesson[] }
    ,
    public createdBy:any
  ) {}
}
