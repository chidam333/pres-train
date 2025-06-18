import { Component, Input, OnInit, AfterViewInit, ElementRef, ViewChild, OnChanges, SimpleChanges } from '@angular/core';
import * as d3 from "d3";

@Component({
  selector: 'app-pie-gender',
  imports: [],
  templateUrl: './pie-gender.html',
  styleUrl: './pie-gender.css'
})
export class PieGender implements OnInit, AfterViewInit, OnChanges {
  @Input() male: number = 0;
  @Input() female: number = 0;
  @ViewChild('pieChart', { static: true }) pieChart!: ElementRef;

  private width = 300;
  private height = 300;
  private radius = Math.min(this.width, this.height) / 2;

  private svg: any;
  private pie = d3.pie<any>().value((d: any) => d.value);
  private arc = d3.arc<any>()
    .innerRadius(0)
    .outerRadius(this.radius - 10);
  
  private color = d3.scaleOrdinal()
    .domain(['Male', 'Female'])
    .range(['blue', '#ED64A6']);

  ngOnInit() {
  }

  ngAfterViewInit() {
    setTimeout(() => {
      this.drawChart();
    }, 100);
  }

  ngOnChanges(changes: SimpleChanges) {
    if ((changes['male'] || changes['female']) && this.pieChart) {
      setTimeout(() => {
        this.drawChart();
      }, 50);
    }
  }

  private drawChart(): void {
    d3.select(this.pieChart.nativeElement).selectAll("*").remove();
    
    const data = [
      { label: 'Male', value: this.male },
      { label: 'Female', value: this.female }
    ].filter(d => d.value > 0);

    if (data.length === 0) {
      return;
    }

    this.svg = d3.select(this.pieChart.nativeElement)
      .append('svg')
      .attr('width', this.width)
      .attr('height', this.height)
      .append('g')
      .attr('transform', `translate(${this.width / 2}, ${this.height / 2})`);

    const pieData = this.pie(data);

    const path = this.svg.selectAll('path')
      .data(pieData)
      .enter()
      .append('path')
      .attr('d', this.arc)
      .attr('fill', (d: any) => this.color(d.data.label))
      .attr('stroke', 'white')
      .attr('stroke-width', 2);

    const text = this.svg.selectAll('text')
      .data(pieData)
      .enter()
      .append('text')
      .attr('transform', (d: any) => `translate(${this.arc.centroid(d)})`)
      .attr('text-anchor', 'middle')
      .attr('font-size', '14px')
      .attr('font-weight', 'bold')
      .attr('fill', 'white')
      .text((d: any) => {
        const total = this.male + this.female;
        const percentage = Math.round((d.data.value / total) * 100);
        return `${percentage}%`;
      });
      
    console.log('Chart drawn successfully');
  }

  get totalCount(): number {
    return this.male + this.female;
  }

  get malePercentage(): number {
    if (this.totalCount === 0) return 0;
    return Math.round((this.male / this.totalCount) * 100);
  }

  get femalePercentage(): number {
    if (this.totalCount === 0) return 0;
    return Math.round((this.female / this.totalCount) * 100);
  }
}
