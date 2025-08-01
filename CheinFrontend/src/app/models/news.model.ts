export interface News {
  newsId: number;
  userId?: number;
  title: string;
  shortDescription: string;
  image?: string;
  content: string;
  createdDate?: Date;
  status?: number;
}

export interface NewsCreate {
  userId: number;
  title: string;
  shortDescription: string;
  image?: string;
  content: string;
  createdDate: Date;
  status: boolean;
}

export interface NewsUpdate {
  newsId: number;
  userId: number;
  title: string;
  shortDescription: string;
  image?: string;
  content: string;
  createdDate: Date;
  status: boolean;
}
