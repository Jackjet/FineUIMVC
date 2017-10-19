
var reportEG = [
	{
	    "icon": "area_ico",
	    "name": "运行日志查询",
	    "dataKey": "egRunLog",
	    "url":'/YCJK/V_Report/RunDayLog'
	},
     {
         "icon": "lineBar_ico",
         "name": "区间压力对比",
         "dataKey": "eg_rangeCompare",
         "url": '/YCJK/V_Report/eg_rangeCompare'
     },
    {
        "icon": "lineBar_ico",
        "name": "综合报表",
        "dataKey": "egCompreReport",
        "url": '/YCJK/V_Report/egCompreReport'
    },
     {
         "icon": "lineBar_ico",
         "name": "实时压力曲线图",
         "dataKey": "egWaterInOutPress",
         "url": '/YCJK/V_Report/egWaterInOutPress'
     },
      {
          "icon": "lineBar_ico",
          "name": "日时段压力曲线图",
          "dataKey": "dayHour_press",
          "url": '/YCJK/V_Report/yearMonthDayChart'
      },
       {
           "icon": "lineBar_ico",
           "name": "日时段电量统计图",
           "dataKey": "dayHour_elec",
           "url": '/YCJK/V_Report/dayHourElec'
       },
       {
           "icon": "lineBar_ico",
           "name": "日时段流量统计图",
           "dataKey": "dayHour_flow",
           "url": '/YCJK/V_Report/dayHourFlow'
       },
       {
           "icon": "lineBar_ico",
           "name": "月每日压力曲线图",
           "dataKey": "monthDay_press",
           "url": '/YCJK/V_Report/MonthDayPress'
       },
        {
            "icon": "lineBar_ico",
            "name": "月每日电量统计图",
            "dataKey": "monthDay_elec",
            "url": '/YCJK/V_Report/MonthDayElec'
        },
         {
             "icon": "lineBar_ico",
             "name": "月每日流量统计图",
             "dataKey": "monthDay_flow",
             "url": '/YCJK/V_Report/MonthDayElec'
         },
          {
               "icon": "lineBar_ico",
               "name": "年每月压力曲线图",
               "dataKey": "yearMonth_press",
               "url": '/YCJK/V_Report/YearMonthPress'
        },
         {
             "icon": "lineBar_ico",
             "name": "年每月电量统计图",
             "dataKey": "yearMonth_elec",
             "url": '/YCJK/V_Report/YearMonthElec'
         },
     {
         "icon": "lineBar_ico",
         "name": "年每月流量统计图",
         "dataKey": "yearMonth_flow",
         "url": '/YCJK/V_Report/YearMonthFlow'
     },
     {
         "icon": "lineBar_ico",
         "name": "吨水能耗",
         "dataKey": "rangePowerUse",
         "url": '/YCJK/V_Report/rangePowerUse'
     },
      {
          "icon": "lineBar_ico",
          "name": "泵房控制",
          "dataKey": "pumpAutoControl",
          "url": '/YCJK/V_Report/pumpAutoControl',
          "size":'500px,300px'
      },
];
var reportYL = [
	{
	    "icon": "area_ico",
	    "name": "运行日志",
	    "url": "/YCJK/V_CDJK/YL_runLog"
	},
    {
        "icon": "lineBar_ico",
        "name": "区间压力对比",
        "url": "/YCJK/V_CDJK/YL_rangeCompare"
    },
    {
        "icon": "lineBar_ico",
        "name": "综合报表",
        "url": "/YCJK/V_CDJK/YL_compreReport"
    }
];


var reportLL = [
	{
	    "icon": "area_ico",
		"name": "运行日志",
		"url": "/YCJK/V_CDJK/Flow_runLog"
	},
    {
        "icon": "lineBar_ico",
        "name": "综合报表",
        "url": "/YCJK/V_CDJK/Flow_compreReport"
    }
];
/*
<li data-key="tfRunLog"><i class="area_ico"></i><a>运行日志</a></li>
                            <li data-key="compreReport"><i class="lineBar_ico"></i><a>综合报表</a></li>
                            <li data-key="waterBoxYW"><i class="lineBar_ico"></i><a>水箱液位</a></li>
                            <li data-key="tfAbility"><i class="lineBar_ico"></i><a>调峰能力</a></li>
                            <li><i class="lineBar_ico"></i><a>月每日压力曲线图</a></li>
                            <li><i class="lineBar_ico"></i><a>月每日电量统计图</a></li>
                            <li><i class="lineBar_ico"></i><a>月每日流量统计图</a></li>
                            <li><i class="lineBar_ico"></i><a>年每月压力曲线图</a></li>
                            <li><i class="lineBar_ico"></i><a>年每月电量统计图</a></li>
                            <li><i class="lineBar_ico"></i><a>年每月流量统计图</a></li>
                            <li><i class="lineBar_ico"></i><a>水泵小保提示报表</a></li>
                            <li><i class="lineBar_ico"></i><a>水泵大保提示报表</a></li>
                            <li><i class="lineBar_ico"></i><a>吨水能耗</a></li>
*/
var reportTF = [
	{
	    "icon": "area_ico",
	    "name": "运行日志",
	    "url": "/YCJK/V_CDJK/TF_runLog"
	},
    {
        "icon": "lineBar_ico",
        "name": "综合报表",
        "url": "/YCJK/V_CDJK/TF_reportWaterInOutPress"
    },
    {
        "icon": "lineBar_ico",
        "name": "水箱液位",
        "url": "/YCJK/V_CDJK/TF_waterBoxYW",
        "dataKey": "waterBoxYW"
    },
     {
         "icon": "lineBar_ico",
         "name": "调峰能力",
         "url": "/YCJK/V_CDJK/TF_waterBoxYW",
         "dataKey": "tfAbility"
     },
      {
          "icon": "lineBar_ico",
          "name": "周每日报表",
          "url": "/YCJK/V_CDJK/TF_weekDayFlow",
          "dataKey": "weekDayFlow"
      }
];
var reportFM = [
	{
	    "icon": "area_ico",
	    "name": "运行日志",
	    "url": "/YCJK/V_CDJK/FM_runLog"
	},
    {
        "icon": "lineBar_ico",
        "name": "综合报表",
        "url": "/YCJK/V_CDJK/FM_compreReport"
    },
    {
        "icon": "lineBar_ico",
        "name": "周每日对比曲线",
        "url": "/YCJK/V_CDJK/FM_weekDayReport"
    },
   {
       "icon": "lineBar_ico",
       "name": "压力设定与开度",
       "url": "/YCJK/V_CDJK/FM_setPressAperture"
   }
];