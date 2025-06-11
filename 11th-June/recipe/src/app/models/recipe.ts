export class Recipe {
    constructor(
        public id: number = 0,
        public name: string = "",
        public cuisine: string = "",
        public cookTimeMinutes: number = 0,
        public ingredients: string[] = []
    ) {}
}

export class RecipeResponse {
    constructor(
        public recipes: Recipe[] = [],
        public total: number = 0,
        public skip: number = 0,
        public limit: number = 0
    ) {}
}
