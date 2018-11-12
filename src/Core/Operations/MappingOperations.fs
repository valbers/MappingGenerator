﻿module MappingOperations

open System
open System.Reflection
open MappingRecords

let private createMappingRules source dest =
    let getProperties (aType:Type) = 
        aType.GetProperties()
        |> Seq.where (fun x -> x.GetIndexParameters() |> Seq.isEmpty)

    let sourceProperties = getProperties source
    let destProperties = (getProperties >> Seq.where (fun x -> x.CanWrite)) dest

    let findFirstPropertyMatching (toBeMatched: PropertyInfo) (sourceProps: PropertyInfo seq) =
        match (sourceProps |> Seq.tryFind (fun x -> String.Compare(x.Name, toBeMatched.Name, ignoreCase = true) = 0)) with
        | Some value -> Some ({ Name = value.Name
                                Type = value.PropertyType }, toBeMatched)
        | None -> None

    let mapTwoPropertyInfosToAMappingRule (source: MappingRuleParticipant , dest: PropertyInfo): MappingRule =
        { Source = source 
          Destination = 
            { Name = dest.Name
              Type = dest.PropertyType } }

    destProperties
    |> Seq.map (fun x -> (sourceProperties |> findFirstPropertyMatching x))
    |> (Seq.filter (fun x -> match x with | Some _ -> true | None -> false) >> Seq.map (fun x -> x.Value))
    |> Seq.map mapTwoPropertyInfosToAMappingRule

let createMapping source dest : Mapping =
    { Source = source
      Destination = dest
      PropertiesMappingRules = createMappingRules source dest }
