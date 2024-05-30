export interface TurbineReadViewModel {
    id: number;
    status: number;
    turbineRadius: number;
    sweptArea: number;
    latitude: number;
    longitude: number;
    altitude: number;
    efficiency: number;
    cutInWindSpeed: number;
    ratedWindSpeed: number;
    shutDownWindSpeed: number;
    windFarmId: number;
}