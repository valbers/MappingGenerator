namespace Mapping

open System
open System.Reflection
open Records

module Operations = 

    let private createMappingRules source dest =
        let getProperties (aType:Type) = 
            aType.GetProperties()
            |> Seq.where (fun x -> x.GetIndexParameters() |> Seq.isEmpty)

        let sourceProperties = getProperties source
        let destProperties = getProperties dest

        let findFirstPropertyMatching (toBeMatched: PropertyInfo) (sourceProps: PropertyInfo seq) =
            match (sourceProps |> Seq.tryFind (fun x -> System.String.Compare(x.Name, toBeMatched.Name, ignoreCase = true) = 0)) with
            | Some value -> (value.Name, value.PropertyType)
            | None -> (source.Name, source)

        let mapTwoPropertyInfosToAMappingRule (source: string*Type, dest: PropertyInfo) =
            let sourceName, sourcePropertyType = source
            { Source = 
                { Name = sourceName
                  Type = sourcePropertyType }
              Destination = 
                { Name = dest.Name
                  Type = dest.PropertyType } }

        destProperties
        |> Seq.where (fun x -> x.CanWrite)
        |> Seq.map (fun x -> ((sourceProperties |> findFirstPropertyMatching x), x))
        |> Seq.map mapTwoPropertyInfosToAMappingRule

    let createMapping source dest : Mapping =
        { Source = source
          Destination = dest
          PropertiesMappingRules = createMappingRules source dest }
