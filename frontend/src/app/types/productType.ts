export interface Product extends BaseProduct {
    productId: number;
  }

  export interface BaseProduct {
    productName: string;
    price: number;
    unitsInStock: number;
    category: string;

  }


  export type Pagination<t> = {
    items: t[];
    totalCount: number;
    pageSize: number;
    pageNumber: number;
    totalPages: number;
    hasNext: boolean;
  };

  export enum SearchProductByField {
    ProductId = 'productId',
    ProductName = 'productName',
    Category = 'category'
  }

  export enum ProductCategory {
    Electronics,
    Clothing,
    Food,
    Books,
    Furniture
  }
  
