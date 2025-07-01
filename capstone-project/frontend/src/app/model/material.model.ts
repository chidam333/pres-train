export class Material {
    constructor(
        public id: number,
        public title: string,
        public description: string | null,
        public filePath: string,
        public fileType: string,
        public lessonId: number,
        public sequenceNo: number,
        public createdAt: Date,
        public updatedAt: Date,
    ) {}

    static fromApiResponse(response: { $id: string; $values: any[] }): Material[] {
        return response.$values.map((item: any) => new Material(
            item.id,
            item.title,
            item.description,
            item.filePath,
            item.fileType,
            item.lessonId,
            item.sequenceNo,
            new Date(item.createdAt),
            new Date(item.updatedAt)
        ));
    }
}
