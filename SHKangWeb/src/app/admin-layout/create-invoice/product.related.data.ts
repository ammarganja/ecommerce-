import { OrderDetail } from "../_models/order.detail.model";

export const orderDetail: OrderDetail = {
    id: 12,
    createdDate: new Date('Jan 07, 2020'),
    orderno: '64616-103',
    user: {
        userid: 1,
        username: 'Shane Warn',
        companyEmail: 'hane.warn@gmail.com',
        phone: '+918888888888'
    },
    address: {
        id: 1,
        address1: 'Cecilia Chapman, 711-2880 Nulla St, Mankato',
        city: 'Fredrick',
        state: 'Nebraska ',
        zipcode: '20620',
    },
    products: [
        {
            id: 1,
            image: '../../../assets/media/products/image1.jpg',
            name: 'Blue Dress',
            style: 'Blue Dress',
            requiredSizeRatio: '5/5/5',
            shippedSizeRatio: '4/4/4',
            remainSizeRatio: '1/1/1',
            qty: 2,
            unitPrice: 90,
            isSelected: false,
            orderSmallRatio: {
                S: '',
                Total: '',
                hasError: false
            },
            orderMideumRatio: {
                M: '',
                Total: '',
                hasError: false
            },
            orderLargeRatio: {
                L: '',
                Total: '',
                hasError: false
            },
            totalAmount: 0
        },
        {
            id: 2,
            image: '../../../assets/media/products/image2.jpg',
            name: 'Purpul Rompers',
            style: 'Purpul Rompers',
            requiredSizeRatio: '5/5/5',
            shippedSizeRatio: '4/4/4',
            remainSizeRatio: '1/1/1',
            qty: 1,
            unitPrice: 449,
            isSelected: false,
            orderSmallRatio: {
                S: '',
                Total: '',
                hasError: false
            },
            orderMideumRatio: {
                M: '',
                Total: '',
                hasError: false
            },
            orderLargeRatio: {
                L: '',
                Total: '',
                hasError: false
            },
            totalAmount: 0
        },
        {
            id: 3,
            image: '../../../assets/media/products/image3.jpg',
            name: 'Colored Dress',
            style: 'Colored Dress',
            requiredSizeRatio: '5/5/5',
            shippedSizeRatio: '4/4/4',
            remainSizeRatio: '1/1/1',
            qty: 1,
            unitPrice: 160,
            isSelected: false,
            orderSmallRatio: {
                S: '',
                Total: '',
                hasError: false
            },
            orderMideumRatio: {
                M: '',
                Total: '',
                hasError: false
            },
            orderLargeRatio: {
                L: '',
                Total: '',
                hasError: false
            },
            totalAmount: 0
        }
    ],
    totalAmount: 0,
    vat: 0,
    shippingCharge: 0,
    TrackingNo: ''
}