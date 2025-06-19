const roles = ["Admin", "User", "Guest"] as const;
type Role = typeof roles[number];

export class UserModel {
    constructor(public username:String, public email:string, public password:string, public role: Role) {}
}