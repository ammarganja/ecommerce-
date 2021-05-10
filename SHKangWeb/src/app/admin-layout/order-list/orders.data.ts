import { Orders } from "../_models/orders.model";

export const orderData: Orders[] = [
    {
        "id": 1,
        "date": "02-09-2021",
        "user": {
            userid: 1,
            username: 'Washington',
            companyEmail: "abc@gmail.com",
            phone: "+918989898989"
        },
        "pono": "W2020",
        "size_ratio": "S/M/L - 2/2/2",
        "ratio": "2/2/2",
        "invoice": [
          {
            id:5027,
            status:3
          },
          {
            id:5028,
            status:4
          }
        ],
        "color": {
            id: 1,
            name: "Yellow",
            status: 1
        },
        "amount": 5000,
        "unitprice": 500,
        "qty": 10,
        "paymentStatus": 1,
        "orderstatus": 1,
        "idproducts": "1,2,3",
        "pendinginvoice": 2
    },
    {
        "id": 2,
        "date": "02-09-2021",
        "user": {
            userid: 2,
            username: 'Shane Worn',
            companyEmail: "shane.worn@gmail.com",
            phone: "+918989898989"
        },
        "pono": "W2020",
        "size_ratio": "S/M/L - 2/2/2",
        "ratio": "2/2/2",
        "invoice": [
          {
            id:5029,
            status:1
          },
          {
            id:5030,
            status:5
          }
        ],
        "color": {
            id: 2,
            name: "Red",
            status: 1
        },
        "amount": 2000,
        "unitprice": 200,
        "qty": 10,
        "paymentStatus": 1,
        "orderstatus": 1,
        "idproducts": "1",
        "pendinginvoice": 2
    },
    {
        "id": 3,
        "date": "02-09-2021",
        "user": {
            userid: 3,
            username: 'Ricky Ponting',
            companyEmail: "ricky@gmail.com",
            phone: "+918989898989"
        },
        "pono": "W2020",
        "size_ratio": "S/M/L - 2/2/2",
        "ratio": "2/2/2",
        "invoice":[
          {
            id:5031,
            status:4
          },
          {
            id:5032,
            status:3
          }
        ],
        "color": {
            id: 3,
            name: "Black",
            status: 1
        },
        "amount": 800,
        "unitprice": 800,
        "qty": 1,
        "paymentStatus": 1,
        "orderstatus": 1,
        "idproducts": "1",
        "pendinginvoice": 2
    },
    {
        "id": 4,
        "date": "02-09-2021",
        "user": {
            userid: 3,
            username: 'Dollar',
            companyEmail: "dollar@gmail.com",
            phone: "+918989898989"
        },
        "pono": "W2020",
        "size_ratio": "S/M/L - 2/2/2",
        "ratio": "2/2/2",
        "invoice": [
          {
            id:5033,
            status:1
          },
          {
            id:5034,
            status:2
          }
        ],
        "color": {
            id: 4,
            name: "Blue",
            status: 1
        },
        "amount": 800,
        "unitprice": 800,
        "qty": 1,
        "paymentStatus": 1,
        "orderstatus": 1,
        "idproducts": "1",
        "pendinginvoice": 2
    }
]

export const  rompres = [
    {
      total: 0,
      selectedProduct: false,
      id: 1,
      img: './assets/media/products/image1.jpg',
      category: 'ROMPERS',
      size_ratio: 'S/M/L - 2/2/2',
      createdDate: '2012/11/10',
      style: 'j11215',
      stock: '100',
      qty: '3',
      color: 'Blue',
      price: 11,
    },
    {
      total: 0,
      selectedProduct: false,
      id: 2,
      img: './assets/media/products/image2.jpg',
      category: 'ROMPERS',
      size_ratio: 'S/M/L - 2/2/2',
      createdDate: '2015/11/11',
      style: 'j11211',
      stock: '100',
      qty: '4',
      color: 'Blue',
      price: 12,
    },
    {
      total: 0,
      selectedProduct: false,
      id: 3,
      img: './assets/media/products/image3.jpg',
      category: 'ROMPERS',
      size_ratio: 'S/M/L - 2/2/1',
      createdDate: '2017/11/12',
      style: 'j11212',
      stock: '100',
      qty: '4',
      color: 'Blue',
      price: 13,
    },
    {
      total: 0,
      selectedProduct: false,
      id: 4,
      img: './assets/media/products/image4.jpg',
      category: 'ROMPERS',
      size_ratio: 'S/M/L - 2/2/2',
      createdDate: '2010/11/13',
      style: 'j11223',
      stock: '100',
      qty: '5',
      color: 'Blue',
      price: 14,
    },
    {
      total: 0,
      selectedProduct: false,
      id: 5,
      img: './assets/media/products/image3.jpg',
      category: 'ROMPERS',
      size_ratio: 'S/M/L - 2/2/2',
      createdDate: '2013/11/14',
      style: 'j11234',
      stock: '100',
      qty: '6',
      color: 'Blue',
      price: 15,
    },
    {
      total: 0,
      selectedProduct: false,
      id: 6,
      img: './assets/media/products/image6.jpg',
      category: 'ROMPERS',
      size_ratio: 'S/M/L - 2/2/2',
      createdDate: '2014/11/15',
      style: 'j11234',
      stock: '100',
      qty: '5',
      color: 'Blue',
      price: 16,
    },
    {
      total: 0,
      selectedProduct: false,
      id: 7,
      img: './assets/media/products/image7.jpg',
      category: 'ROMPERS',
      size_ratio: 'S/M/L - 2/2/2',
      createdDate: '2017/11/16',
      style: 'j11224',
      stock: '100',
      qty: '5',
      color: 'Blue',
      price: 17,
    },
    {
      total: 0,
      selectedProduct: false,
      id: 8,
      img: './assets/media/products/image8.jpg',
      category: 'ROMPERS',
      size_ratio: 'S/M/L - 2/1/2',
      createdDate: '2019/11/17',
      style: 'j11234',
      stock: '100',
      qty: '5',
      color: 'Blue',
      price: 18,
    },
    {
      total: 0,
      selectedProduct: false,
      id: 9,
      img: './assets/media/products/image9.jpg',
      category: 'ROMPERS',
      size_ratio: 'S/M/L - 2/2/2',
      createdDate: '2021/11/01',
      style: 'j11245',
      stock: '100',
      qty: '5',
      color: 'Blue',
      price: 19,
    },
    {
      total: 0,
      selectedProduct: false,
      id: 10,
      img: './assets/media/products/image10.jpg',
      category: 'ROMPERS',
      size_ratio: 'S/M/L - 2/2/2',
      createdDate: '2020/11/19',
      style: 'j11232',
      stock: '100',
      qty: '5',
      color: 'Blue',
      price: 20,
    },
  ];
 export const dresses = [
    {
      total: 0,
      selectedProduct: false,
      id: 11,
      img: './assets/media/products/image1.jpg',
      category: 'DRESSES',
      size_ratio: 'S/M/L - 2/2/2',
      createdDate: '2012/11/10',
      style: 'j11215',
      stock: '100',
      qty: '',
      color: 'Blue',
      price: 21,
    },
    {
      total: 0,
      selectedProduct: false,
      id: 12,
      img: './assets/media/products/image2.jpg',
      category: 'DRESSES',
      size_ratio: 'S/M/L - 2/2/2',
      createdDate: '2015/11/11',
      style: 'j11211',
      stock: '100',
      qty: '',
      color: 'Blue',
      price: 22,
    },
  ];
  export const data = [
    {
      id: 1,
      first_name: 'Olive',
      last_name: 'Yew',
      email: 'test1@test.com',
      gender: 'male',
      status: 'active',
      address: '4040  Birch  Street',
    },
    {
      id: 2,
      first_name: 'Aida',
      last_name: 'Bugg',
      email: 'test2@test.com',
      gender: 'male',
      status: 'active',
      address: '4356  Florence Street',
    },
    {
      id: 3,
      first_name: 'Maureen',
      last_name: 'Biologist',
      email: 'test3@test.com',
      gender: 'male',
      status: 'active',
      address: '4625  Birch  Street',
    },
    {
      id: 4,
      first_name: 'Teri',
      last_name: 'Dactyl',
      email: 'test4@test.com',
      gender: 'male',
      status: 'active',
      address: '4625  Birch  Street',
    },
    {
      id: 5,
      first_name: 'Peg',
      last_name: 'Legge',
      email: 'test5@test.com',
      gender: 'male',
      status: 'active',
      address: '4356  Florence Street',
    },
    {
      id: 6,
      first_name: 'Liz',
      last_name: 'Erd',
      email: 'test6@test.com',
      gender: 'male',
      status: 'active',
      address: '4625  Birch  Street',
    },
    {
      id: 7,
      first_name: 'Constance',
      last_name: 'Noring',
      email: 'test7@test.com',
      gender: 'male',
      status: 'active',
      address: '4356  Florence Street',
    },
    {
      id: 8,
      first_name: 'Lynn',
      last_name: 'O’Leeum',
      email: 'test8@test.com',
      gender: 'male',
      status: 'active',
      address: '4356 Tree Street',
    },
    {
      id: 9,
      first_name: 'Isabelle',
      last_name: 'Ringing',
      email: 'test9@test.com',
      gender: 'male',
      status: 'active',
      address: '4356 Florence Street',
    },
    {
      id: 10,
      first_name: 'Eileen',
      last_name: 'Sideways',
      email: 'test10@test.com',
      gender: 'male',
      status: 'active',
      address: '4356 Tree Street',
    },
    {
      id: 11,
      first_name: 'Rita',
      last_name: '',
      email: 'test11@test.com',
      gender: 'male',
      status: 'active',
      address: '4356  Florence Street',
    },
  ];
