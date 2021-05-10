import { Component, OnInit, ViewChild } from '@angular/core';
import { DashboardService } from 'src/app/admin-layout/services/dashboard.service';
import {
  ChartComponent,
  ApexAxisChartSeries,
  ApexChart,
  ApexXAxis,
  ApexDataLabels,
  ApexTitleSubtitle,
  ApexStroke,
  ApexGrid,
  ApexFill,
  ApexMarkers,
  ApexYAxis
} from "ng-apexcharts";

export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  dataLabels: ApexDataLabels;
  grid: ApexGrid;
  fill: ApexFill;
  markers: ApexMarkers;
  yaxis: ApexYAxis;
  stroke: ApexStroke;
  title: ApexTitleSubtitle;
};


@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})

export class DashboardComponent {

  //#region Variable Declaration
  @ViewChild("chart") chart: ChartComponent;
  public chartOptionsOrders: Partial<ChartOptions>;
  public chartOptionsCustomers: Partial<ChartOptions>;
  public dashboardData: any;
  public errorMessage: string;
  public isLoading: boolean;
  public orderCategories: Array<string>;
  public orderData: Array<number>;
  public userCategories: Array<string>;
  public userData: Array<number>;
  //#endregion

  //#region Constcructor
  constructor(private dashboardService: DashboardService) {
    this.isLoading = false;

    this.chartOptionsOrders = {
      series: [
        {
          name: "Orders",
          data: []
        }
      ],
      chart: {
        height: 350,
        type: "line"
      },
      stroke: {
        width: 7,
        curve: "smooth"
      },
      xaxis: {
        type: "category",
        categories: []
      },
      fill: {
        type: "gradient",
        gradient: {
          shade: "dark",
          gradientToColors: ["#FDD835"],
          shadeIntensity: 1,
          type: "horizontal",
          opacityFrom: 1,
          opacityTo: 1,
          stops: [0, 100, 100, 100]
        }
      },
      markers: {
        size: 4,
        colors: ["#FFA41B"],
        strokeColors: "#fff",
        strokeWidth: 2,
        hover: {
          size: 7
        }
      },
      yaxis: {
        min: 0,
        //max: Math.max(...this.orderData),
        max: 40,
        title: {
          text: "Total Number of Orders"
        }
      }
    };

    this.chartOptionsCustomers = {
      series: [
        {
          name: "Customers",
          data: []
        }
      ],
      chart: {
        height: 350,
        type: "line"
      },
      stroke: {
        width: 7,
        curve: "smooth"
      },
      xaxis: {
        type: "category",
        categories: []
      },
      fill: {
        type: "gradient",
        gradient: {
          shade: "dark",
          gradientToColors: ["#FDD835"],
          shadeIntensity: 1,
          type: "horizontal",
          opacityFrom: 1,
          opacityTo: 1,
          stops: [0, 100, 100, 100]
        }
      },
      markers: {
        size: 4,
        colors: ["#FFA41B"],
        strokeColors: "#fff",
        strokeWidth: 2,
        hover: {
          size: 7
        }
      },
      yaxis: {
        min: 0,
        max: 40,
        title: {
          text: "Total Number of Customers"
        }
      }
    };
  }
  //#endregion

  //#region Methods

  /**
   * NgOnInit
   */
  ngOnInit() {
    this.isLoading = true;
    this.dashboardService.dashboardData().subscribe((res: any) => {
      this.isLoading = false;
      if (res.result) {
        this.dashboardData = res.data;
        if (this.dashboardData.monthwiseOrder) {
          this.getOrderChart();
        }
        if (this.dashboardData.monthwiseUser) {
          this.getCustomeChart();
        }
      } else {
        this.errorMessage = res.message;
      }
    })
  }

  /**
   * GetOrderChart
   */
  getOrderChart() {
      this.orderData = new Array<number>();
      this.orderCategories = new Array<string>();
      this.dashboardData.monthwiseOrder.forEach(element => {
        this.orderData.push(element.totalOrder);
        this.orderCategories.push(element.month)
      });
      this.chartOptionsOrders.series = [{
        data: this.orderData
      }];
      this.chartOptionsOrders.xaxis.categories = this.orderCategories;
  }

  /**
   * GetCustomerChart
   */
  getCustomeChart() {
    this.userData = new Array<number>();
    this.userCategories = new Array<string>();
    this.dashboardData.monthwiseUser.forEach(element => {
      this.userData.push(element.totalUser);
      this.userCategories.push(element.month)
    });
    this.chartOptionsCustomers.series = [{
      data : this.userData
    }];
    this.chartOptionsCustomers.xaxis.categories = this.userCategories;
  }
  //#endregion
}

