export enum CoursePlanningRouteEnum {
  CoursePlannings = "coursePlannings",
  CustomCoursePlannings = "customCoursePlannings"
}

// tslint:disable-next-line:typedef
export function IsEqual(coursePlanningRoute: number, coursePlanningRouteEnum: CoursePlanningRouteEnum) {
  return CoursePlanningRouteEnum[coursePlanningRoute] === CoursePlanningRouteEnum[coursePlanningRouteEnum];
}
