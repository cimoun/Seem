import { Tag } from './task.model';

export interface Space {
  id: string;
  name: string;
  description?: string;
  icon?: string;
  sortOrder: number;
  articleCount: number;
  createdAt: string;
}

export interface Article {
  id: string;
  title: string;
  content: string;
  slug: string;
  isPinned: boolean;
  spaceId: string;
  parentArticleId?: string;
  depth: number;
  sortOrder: number;
  childArticles: Article[];
  tags: Tag[];
  createdAt: string;
  updatedAt?: string;
}

export interface ArticleRevision {
  id: string;
  articleId: string;
  previousContent: string;
  changeNote?: string;
  createdAt: string;
}
