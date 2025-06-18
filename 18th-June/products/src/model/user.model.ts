class Address{
    constructor(
        public city: string,
        public state: string,
    ) {}
}
class Hair {
    constructor(
        public color: string,
        public type: string
    ) {}
}
export class UserModel {
    constructor(
        public id: number,
        public firstName: string,
        public lastName: string,
        public gender: string,
        public email: string,
        public role: string,
        public address: Address,
        public hair: Hair,
    ) {}
}